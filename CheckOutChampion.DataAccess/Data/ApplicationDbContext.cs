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
        }
    }
}
