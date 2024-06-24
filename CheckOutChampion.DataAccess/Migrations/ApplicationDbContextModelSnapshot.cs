﻿// <auto-generated />
using CheckOutChampion.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheckOutChampion.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CheckOutChampion.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Automotive"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Beauty & Personal Care"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Books & Media"
                        },
                        new
                        {
                            Id = 4,
                            DisplayOrder = 4,
                            Name = "Clothing & Apparel"
                        },
                        new
                        {
                            Id = 5,
                            DisplayOrder = 5,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 6,
                            DisplayOrder = 6,
                            Name = "Health & Wellness"
                        },
                        new
                        {
                            Id = 7,
                            DisplayOrder = 7,
                            Name = "Home & Garden"
                        },
                        new
                        {
                            Id = 8,
                            DisplayOrder = 8,
                            Name = "Pet Supplies"
                        },
                        new
                        {
                            Id = 9,
                            DisplayOrder = 9,
                            Name = "Sports & Outdoors"
                        },
                        new
                        {
                            Id = 10,
                            DisplayOrder = 10,
                            Name = "Toys & Games"
                        });
                });

            modelBuilder.Entity("CheckOutChampion.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "High-performance car wax for a long-lasting shine.",
                            Price = 19.989999999999998,
                            ProductName = "Car Wax"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Description = "Gentle facial cleanser for all skin types.",
                            Price = 12.49,
                            ProductName = "Facial Cleanser"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Description = "A gripping mystery novel that will keep you on the edge of your seat.",
                            Price = 8.9900000000000002,
                            ProductName = "Mystery Novel"
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 4,
                            Description = "Comfortable cotton T-shirt available in various colors.",
                            Price = 15.0,
                            ProductName = "Men's T-Shirt"
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 5,
                            Description = "Latest model with cutting-edge features.",
                            Price = 699.99000000000001,
                            ProductName = "Smartphone"
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 6,
                            Description = "Daily multivitamins to boost your health.",
                            Price = 25.5,
                            ProductName = "Vitamins"
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 7,
                            Description = "Complete set of tools for your gardening needs.",
                            Price = 45.0,
                            ProductName = "Garden Tools Set"
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 8,
                            Description = "Comfortable and durable pet bed for your furry friends.",
                            Price = 35.0,
                            ProductName = "Pet Bed"
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 9,
                            Description = "Spacious tent for outdoor adventures.",
                            Price = 120.0,
                            ProductName = "Camping Tent"
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 10,
                            Description = "Fun and engaging board game for family and friends.",
                            Price = 29.989999999999998,
                            ProductName = "Board Game"
                        });
                });

            modelBuilder.Entity("CheckOutChampion.Models.Product", b =>
                {
                    b.HasOne("CheckOutChampion.Models.Category", "CategoryNav")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryNav");
                });
#pragma warning restore 612, 618
        }
    }
}
