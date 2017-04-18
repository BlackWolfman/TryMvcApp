using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TryMvcApp.Helpers;

namespace TryMvcApp.Controllers
{
    [Authorize]
    public class UserController : MainController
    {
        public ActionResult Index()
        {
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

        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View(CurrentUser.Cart.Products.AsEnumerable());
        }

        public ActionResult Wishlist()
        {
            return View(CurrentUser.Wishlist.Products.AsEnumerable());
        }

        public ActionResult AddToCart(int? id)
        {
            if (CurrentUser.Cart.Products.Any(product => product.Id == id))
            {
                CurrentUser.Cart.Products.First(product => product.Id == id).Amount++;
                
                return RedirectToAction("Details", "Product", new { id });
            }

            CurrentUser.Cart.Products.Add(AppDb.Products.Find(id));
            
            return RedirectToAction("Details", "Product", new { id });
        }

        public ActionResult AddToWishlist(int? id)
        {
            if (CurrentUser.Wishlist.Products.Any(product => product.Id == id))
            {
                return RedirectToAction("List", "Product");
            }

            CurrentUser.Wishlist.Products.Add(AppDb.Products.Find(id));

            return RedirectToAction("Details", "Product", new { id });
        }

        public ActionResult RemoveFromCart(int? id)
        {
            if (CurrentUser.Cart.Products.All(product => product.Id != id))
            {
                return RedirectToAction("Cart", "User");
            }

            if (CurrentUser.Cart.Products.First(product => product.Id == id).Amount > 1)
            {
                CurrentUser.Cart.Products.First(product => product.Id == id).Amount--;

                return RedirectToAction("Cart", "User");
            }

            CurrentUser.Cart.Products.Remove(CurrentUser.Cart.Products.FirstOrDefault(product => product.Id == id));

            return RedirectToAction("Cart", "User");
        }

        public ActionResult RemoveFromWishlist(int? id)
        {
            if (CurrentUser.Wishlist.Products.All(product => product.Id != id))
            {
                return RedirectToAction("Wishlist", "User");
            }

            CurrentUser.Wishlist.Products.Remove(CurrentUser.Wishlist.Products.FirstOrDefault(product => product.Id == id));

            return RedirectToAction("Wishlist", "User");
        }

    }
}