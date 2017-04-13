using System.ComponentModel.DataAnnotations;

namespace TryMvcApp.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        public virtual CartModel Cart { get; set; }

        public virtual WishlistModel Wishlist { get; set; }

        public string Role { set; get; }

        [Required(ErrorMessage = "Username is required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Password must be at least 3 symbols long!")]
        public string Password { get; set; }
    }
}