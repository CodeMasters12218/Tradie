using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tradie.Models;
using Tradie.Models.Orders;
using Tradie.Models.Products;
using Tradie.Models.ShoppingCart;
using Tradie.Models.UserAddressModel;
using Tradie.Models.UserCards;
using Tradie.Models.Users;
using Tradie.Models.Wishlist;

namespace Tradie.Data
{
	public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Subcategory> Subcategories { get; set; }
		public new DbSet<User> Users { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Wishlist> Wishlists { get; set; }
		public DbSet<WishlistItem> WishlistItems { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<UserCardModel> UserCards { get; set; }
		public DbSet<UsersAddressModel> UserAddress { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserCardProfileModel>().HasNoKey(); // Asegurar que no se trata como tabla

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Seller)
				.WithMany(u => u.Products)
				.HasForeignKey(p => p.SellerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>()
				.HasDiscriminator<string>("UserType")
				.HasValue<Admin>("Admin")
				.HasValue<Seller>("Seller")
				.HasValue<Customer>("Customer");

			modelBuilder.Entity<Order>()
				.HasOne(o => o.Customer)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.CustomerId);

			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Order)
				.WithMany(o => o.Items)
				.HasForeignKey(oi => oi.OrderId);

			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Product)
				.WithMany(p => p.OrderItems)
				.HasForeignKey(oi => oi.ProductId);

			modelBuilder.Entity<Review>()
				.HasOne(r => r.Product)
				.WithMany(p => p.Reviews)
				.HasForeignKey(r => r.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Review>()
				.HasOne(r => r.Customer)
				.WithMany(c => c.Reviews)
				.HasForeignKey(r => r.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Wishlist>()
				.HasMany(w => w.Items)
				.WithOne(i => i.Wishlist)
				.HasForeignKey(i => i.WishlistId);

			modelBuilder.Entity<ShoppingCart>()
				.HasMany(c => c.Items)
				.WithOne(i => i.ShoppingCart)
				.HasForeignKey(i => i.ShoppingCartId);
		}

	}
}
