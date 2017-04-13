using System.Data.Entity;

namespace TryMvcApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        : base("AppDbContext")
        {
        //    Database.SetInitializer<AppDbContext>(new DropCreateDatabaseAlways<AppDbContext>());
            Database.SetInitializer<AppDbContext>(null);
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartModel>()
                .HasRequired(x => x.User)
                .WithOptional(x => x.Cart);

            modelBuilder.Entity<WishlistModel>()
                .HasRequired(x => x.User)
                .WithOptional(x => x.Wishlist);

            modelBuilder.Entity<ProductModel>()
                .HasMany(x => x.Wishlists)
                .WithMany(x => x.Products);

            modelBuilder.Entity<ProductModel>()
                .HasMany(x => x.Carts)
                .WithMany(x => x.Products);

            base.OnModelCreating(modelBuilder);
        }
    
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<ProductModel> Products { get; set; }
        public virtual DbSet<CartModel> Carts { get; set; }
        public virtual DbSet<WishlistModel> Wishlists { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose();
            }
            base.Dispose(disposing);
        }
    }
}