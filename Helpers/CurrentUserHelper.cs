using System;
using System.Collections.Generic;
using TryMvcApp.Models;

namespace TryMvcApp.Helpers
{
    public class CurrentUserHelper
    {
        public static List<UserModel> OnlineUsers = new List<UserModel>();

        public static void AddOnlineUser(UserModel userLogin)
        {
            OnlineUsers.Add(userLogin);
        }

        public static void RemoveOnlineUser(string userLogoutName)
        {
            OnlineUsers.Remove(OnlineUsers?.Find(user => string.Equals(user.UserName, userLogoutName, StringComparison.CurrentCultureIgnoreCase)));
        }

        public static UserModel GetCurrentUser(string currentUserName)
        {
            return OnlineUsers?.Find(user => string.Equals(user.UserName, currentUserName, StringComparison.OrdinalIgnoreCase));
        }

        public static float GetCartSummary()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return 0;
            }

            ICollection<ProductModel> currentUserCart = GetCurrentUser(System.Web.HttpContext.Current.User.Identity.Name).Cart.Products;

            float cartSummary = 0;

            foreach (ProductModel product in currentUserCart)
            {
                cartSummary += (float) product.Price * product.Amount;
            }

            return cartSummary;
        }
    }
}