using System.Web.Mvc;

namespace TryMvcApp.Controllers
{
    public class HomeController : MainController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}