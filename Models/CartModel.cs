using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TryMvcApp.Models
{
    public class CartModel
    {
        [Key,ForeignKey("User")]
        public int UserId { get; set; }
 
        [Required]
        public virtual UserModel User { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }
    }
}