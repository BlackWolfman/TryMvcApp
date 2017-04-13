using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using TryMvcApp.Models;

namespace TryMvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly AppDbContext _appDb = new AppDbContext();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported != true) return;
            if (Request.Cookies[FormsAuthentication.FormsCookieName] == null) return;
            try
            {              
                var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                if (formsAuthenticationTicket == null) return;
                var userName = formsAuthenticationTicket.Name;

                var role = string.Empty;
                var user = _appDb.Users.SingleOrDefault(u => u.UserName == userName);

                if (user != null) role = user.Role;

                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(userName, "Forms"), role.Split(';'));
            }
            catch (Exception)
            {
                //somehting went wrong
            }
        }
    }
}
