using System.Web.Mvc;
using TryMvcApp.Helpers;
using TryMvcApp.Models;

namespace TryMvcApp.Controllers
{
    public class MainController : Controller
    {
        public AppDbContext AppDb;

        public static UserModel CurrentUser = CurrentUserHelper.GetCurrentUser(System.Web.HttpContext.Current.User.Identity.Name);

        public MainController()
        {
            AppDb = new AppDbContext();
        }
    }
}