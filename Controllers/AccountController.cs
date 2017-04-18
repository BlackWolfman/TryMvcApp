using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TryMvcApp.Helpers;
using TryMvcApp.Models;

namespace TryMvcApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : MainController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel newUser)
        {
            if (!ModelState.IsValid) return View();
            string userName = newUser.UserName;

            if (!AppDb.Users.Any(user => user.UserName == userName))
            {
                newUser.Password = FunctionHelper.DoMd5Hash(newUser.Password);

                UserModel user = AppDb.Users.Add(newUser);
                user.Cart = AppDb.Carts.Add(new CartModel());
                user.Wishlist = AppDb.Wishlists.Add(new WishlistModel());

                AppDb.SaveChanges();
                return RedirectToAction("Login", "Account");
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

            string userName = userLogin.UserName;
            string hashedPassword = FunctionHelper.DoMd5Hash(userLogin.Password);

            bool userValid = AppDb.Users.Any(user => user.UserName == userName && user.Password == hashedPassword);

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

        public ActionResult UserList()
        {
            return View(AppDb.Users);
        }

        //
        //For testing purpose
        public ActionResult CreateData()
        {
            ProductModel product1 = AppDb.Products.Add(new ProductModel() { Name = "Cooler_1", Category = "Cooling", Price = 50, OldPrice = 65, Description = "Description for Cooler #1" });
            ProductModel product2 = AppDb.Products.Add(new ProductModel() { Name = "GPU_1", Category = "GraphicsCards", Price = 250, OldPrice = 250, Description = "Description for GPU #1" });
            ProductModel product3 = AppDb.Products.Add(new ProductModel() { Name = "MemoryCard_1", Category = "MemoryCards", Price = 40, OldPrice = 45, Description = "Description for Memory Card #1" });
            ProductModel product4 = AppDb.Products.Add(new ProductModel() { Name = "Monitor_1", Category = "Monitors", Price = 400, OldPrice = 520, Description = "Description for Monitor #1" });
            ProductModel product5 = AppDb.Products.Add(new ProductModel() { Name = "Motherboard_1", Category = "Motherboards", Price = 130, OldPrice = 150, Description = "Description for Motherboard #1" });
            ProductModel product6 = AppDb.Products.Add(new ProductModel() { Name = "PowerSupply_1", Category = "PowerSupplies", Price = 80, OldPrice = 100, Description = "Description for Power Supply #1" });
            ProductModel product7 = AppDb.Products.Add(new ProductModel() { Name = "CPU_1", Category = "Processors", Price = 200, OldPrice = 235, Description = "Description for CPU #1" });

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

            return RedirectToAction("UserList", "Account");
        }
    }
}