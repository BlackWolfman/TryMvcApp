using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TryMvcApp.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        
        public virtual ICollection<CartModel> Carts { get; set; }

        public virtual ICollection<WishlistModel> Wishlists { get; set; }
        
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Category { get; set; }

        [Range(1, 2000)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Range(1, 2000)]
        [DataType(DataType.Currency)]
        public decimal OldPrice { get; set; }

        public string Description { get; set; }

        [Required]
        public int Amount { get; set; } = 1;
    }
}