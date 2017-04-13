using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TryMvcApp.Models;
using TryMvcApp.Helpers;

namespace TryMvcApp.Controllers
{
    public class UserController : MainController
    {
        private UserModel _currentUser = null;

        public ActionResult Index()
        {
            return RedirectToAction("Account", "User");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel newUser)
        {
            if (!ModelState.IsValid) return View();
            var userName = newUser.UserName;

            if (!AppDb.Users.Any(user => user.UserName == userName))
            {
                newUser.Password = FunctionHelper.DoMd5Hash(newUser.Password);

                var user = AppDb.Users.Add(newUser);
                user.Cart = AppDb.Carts.Add(new CartModel());
                user.Wishlist = AppDb.Wishlists.Add(new WishlistModel());

                AppDb.SaveChanges();
                return RedirectToAction("Login", "User");
            }

            ModelState.AddModelError("", "The user with such User Name already exists!");
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel userLogin)
        {
            if (!ModelState.IsValid) return View(userLogin);
            var userName = userLogin.UserName;
            var hashedPassword = FunctionHelper.DoMd5Hash(userLogin.Password);

            var userValid = AppDb.Users.Any(user => user.UserName == userName && user.Password == hashedPassword);

            if (!userValid)
            {
                ModelState.AddModelError("", "The User Name or Password provided is incorrect!");
                return View();
            }

            userLogin = AppDb.Users.FirstOrDefault(user => user.UserName == userName);
            FormsAuthentication.SetAuthCookie(userName, false);

            CurrentUserHelper.AddOnlineUser(userLogin);

            return RedirectToAction("Account", "User");
        }

        public ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            CurrentUserHelper.RemoveOnlineUser(User.Identity.Name);

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Account()
        {
            return View();
        }

        [Authorize]
        public ActionResult Cart()
        {
            _currentUser = CurrentUserHelper.GetCurrentUser(User.Identity.Name);

            return View(_currentUser.Cart.Products.AsEnumerable());
        }

        [Authorize]
        public ActionResult Wishlist()
        {
            _currentUser = CurrentUserHelper.GetCurrentUser(User.Identity.Name);

            return View(_currentUser.Wishlist.Products.AsEnumerable());
        }

        [Authorize]
        public ActionResult AddToCart(int? id)
        {
            _currentUser = CurrentUserHelper.GetCurrentUser(User.Identity.Name);

            if (_currentUser.Cart.Products.Any(product => product.Id == id))
            {
                return RedirectToAction("List", "Product");
            }

            _currentUser.Cart.Products.Add(AppDb.Products.Find(id));
            
            return RedirectToAction("Details", "Product", new { id = id });
        }

        [Authorize]
        public ActionResult AddToWishlist(int? id)
        {
            _currentUser = CurrentUserHelper.GetCurrentUser(User.Identity.Name);

            if (_currentUser.Wishlist.Products.Any(product => product.Id == id))
            {
                return RedirectToAction("List", "Product");
            }

            _currentUser.Wishlist.Products.Add(AppDb.Products.Find(id));

            return RedirectToAction("Details", "Product", new { id = id });
        }

        [Authorize]
        public ActionResult RemoveFromCart(int? id)
        {
            _currentUser = CurrentUserHelper.GetCurrentUser(User.Identity.Name);

            if (_currentUser.Cart.Products.All(product => product.Id != id))
            {
                return RedirectToAction("Cart", "User");
            }

            _currentUser.Cart.Products.Remove(_currentUser.Cart.Products.FirstOrDefault(product => product.Id == id));

            return RedirectToAction("Cart", "User");
        }

        [Authorize]
        public ActionResult RemoveFromWishlist(int? id)
        {
            _currentUser = CurrentUserHelper.GetCurrentUser(User.Identity.Name);

            if (_currentUser.Wishlist.Products.All(product => product.Id != id))
            {
                return RedirectToAction("Wishlist", "User");
            }

            _currentUser.Wishlist.Products.Remove(_currentUser.Wishlist.Products.FirstOrDefault(product => product.Id == id));

            return RedirectToAction("Wishlist", "User");
        }

        public ActionResult UserList()
        {
            return View(AppDb.Users);
        }

        public ActionResult CreateData()
        {
            var product1 = AppDb.Products.Add(new ProductModel() { Name = "Cooler_1", Category = "Cooling", Price = 50, OldPrice = 65, Description = "Description for Cooler #1" });
            var product2 = AppDb.Products.Add(new ProductModel() { Name = "GPU_1", Category = "GraphicsCards", Price = 250, OldPrice = 250, Description = "Description for GPU #1" });
            var product3 = AppDb.Products.Add(new ProductModel() { Name = "MemoryCard_1", Category = "MemoryCards", Price = 40, OldPrice = 45, Description = "Description for Memory Card #1" });
            var product4 = AppDb.Products.Add(new ProductModel() { Name = "Monitor_1", Category = "Monitors", Price = 400, OldPrice = 520, Description = "Description for Monitor #1" });
            var product5 = AppDb.Products.Add(new ProductModel() { Name = "Motherboard_1", Category = "Motherboards", Price = 130, OldPrice = 150, Description = "Description for Motherboard #1" });
            var product6 = AppDb.Products.Add(new ProductModel() { Name = "PowerSupply_1", Category = "PowerSupplies", Price = 80, OldPrice = 100, Description = "Description for Power Supply #1" });
            var product7 = AppDb.Products.Add(new ProductModel() { Name = "CPU_1", Category = "Processors", Price = 200, OldPrice = 235, Description = "Description for CPU #1" });

            AppDb.Users.Add(new UserModel()
            {
                Cart = AppDb.Carts.Add(new CartModel() { Products = new List<ProductModel>() { product1, product2 } }),
                Wishlist = AppDb.Wishlists.Add(new WishlistModel() { Products = new List<ProductModel>() { product4, product3, product2 } }),
                UserName = "User1",
                Password = FunctionHelper.DoMd5Hash("pass1"),
                Role = "User"
            });
            AppDb.Users.Add(new UserModel()
            {
                Cart = AppDb.Carts.Add(new CartModel() { Products = new List<ProductModel>() { product4, product5 } }),
                Wishlist = AppDb.Wishlists.Add(new WishlistModel() { Products = new List<ProductModel>() { product7, product6, product5 } }),
                UserName = "Admin1",
                Password = FunctionHelper.DoMd5Hash("pass2"),
                Role = "Admin"
            });
            AppDb.Users.Add(new UserModel()
            {
                Cart = AppDb.Carts.Add(new CartModel()),
                Wishlist = AppDb.Wishlists.Add(new WishlistModel()),
                UserName = "User2",
                Password = FunctionHelper.DoMd5Hash("pass3"),
                Role = "User"
            });

            AppDb.SaveChanges();

            return RedirectToAction("UserList", "User");
        }

    }
}