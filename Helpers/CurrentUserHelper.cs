using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using TryMvcApp.Models;

namespace TryMvcApp.Helpers
{
    public class CurrentUserHelper
    {
        public static List<UserModel> OnlineUsers = new List<UserModel>();
        public static int OnlineUsersCount = 0;

        public static void AddOnlineUser(UserModel userLogin)
        {
            OnlineUsers.Add(userLogin);
            OnlineUsersCount++;
        }

        public static void RemoveOnlineUser(string userLogoutName)
        {
            OnlineUsers.Remove(OnlineUsers?.Find(user => string.Equals(user.UserName, userLogoutName, StringComparison.CurrentCultureIgnoreCase)));
            OnlineUsersCount--;
        }

        public static UserModel GetCurrentUser(string currentUserName)
        {
            return OnlineUsers?.Find(user => string.Equals(user.UserName, currentUserName, StringComparison.OrdinalIgnoreCase));
        }

        public static float GetCartSummary()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return 00;
            }

            var currentUserCart = GetCurrentUser(System.Web.HttpContext.Current.User.Identity.Name).Cart.Products;

            float cartSummary = 0;

            foreach (var product in currentUserCart)
            {
                cartSummary += (float) product.Price;
            }

            return cartSummary;
        }
    }
}