﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheBookHeaven;

#nullable disable

namespace TheBookHeaven.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20250613014442_ReSeedBooks")]
    partial class ReSeedBooks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TheBookHeaven.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Best Sellers",
                            ImageUrl = "https://cdn.britannica.com/04/126004-050-EC4DF54F/Dustcover-Louisa-May-Alcott-Little-Women-novel.jpg",
                            Price = 19.99m,
                            Title = "Little Women"
                        },
                        new
                        {
                            Id = 2,
                            Category = "Fiction",
                            ImageUrl = "https://assets.lulu.com/cover_thumbs/8/d/8dzdnj-front-shortedge-384.jpg",
                            Price = 14.99m,
                            Title = "The Great Gatsby"
                        },
                        new
                        {
                            Id = 3,
                            Category = "Non-Fiction",
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1703329310i/23692271.jpg",
                            Price = 21.50m,
                            Title = "Sapiens"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
