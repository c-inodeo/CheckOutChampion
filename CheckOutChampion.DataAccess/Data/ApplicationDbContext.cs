using CheckOutChampion.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckOutChampion.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Automotive", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Beauty & Personal Care", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Books & Media", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Clothing & Apparel", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Electronics", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Health & Wellness", DisplayOrder = 6 },
                new Category { Id = 7, Name = "Home & Garden", DisplayOrder = 7 },
                new Category { Id = 8, Name = "Pet Supplies", DisplayOrder = 8 },
                new Category { Id = 9, Name = "Sports & Outdoors", DisplayOrder = 9 },
                new Category { Id = 10, Name = "Toys & Games", DisplayOrder = 10 }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    ProductName = "Car Wax",
                    Description = "High-performance car wax for a long-lasting shine.",
                    Price = 19.99,
                    CategoryId = 1,
                    ImageUrl =""
                },
                new Product
                {
                    Id = 2,
                    ProductName = "Facial Cleanser",
                    Description = "Gentle facial cleanser for all skin types.",
                    Price = 12.49,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    ProductName = "Mystery Novel",
                    Description = "A gripping mystery novel that will keep you on the edge of your seat.",
                    Price = 8.99,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    ProductName = "Men's T-Shirt",
                    Description = "Comfortable cotton T-shirt available in various colors.",
                    Price = 15.00,
                    CategoryId = 4,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    ProductName = "Smartphone",
                    Description = "Latest model with cutting-edge features.",
                    Price = 699.99,
                    CategoryId = 5,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    ProductName = "Vitamins",
                    Description = "Daily multivitamins to boost your health.",
                    Price = 25.50,
                    CategoryId = 6,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 7,
                    ProductName = "Garden Tools Set",
                    Description = "Complete set of tools for your gardening needs.",
                    Price = 45.00,
                    CategoryId = 7,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 8,
                    ProductName = "Pet Bed",
                    Description = "Comfortable and durable pet bed for your furry friends.",
                    Price = 35.00,
                    CategoryId = 8,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 9,
                    ProductName = "Camping Tent",
                    Description = "Spacious tent for outdoor adventures.",
                    Price = 120.00,
                    CategoryId = 9,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 10,
                    ProductName = "Board Game",
                    Description = "Fun and engaging board game for family and friends.",
                    Price = 29.99,
                    CategoryId = 10,
                    ImageUrl = ""
                });
        }
    }
}
