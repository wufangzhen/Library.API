﻿// <auto-generated />
using System;
using Library.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Library.API.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    partial class LibraryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("Library.API.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"),
                            BirthDate = new DateTimeOffset(new DateTime(1960, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Beijing",
                            Email = "author1@xxx.com",
                            Name = "Author 1"
                        },
                        new
                        {
                            Id = new Guid("7d04a48e-be4e-468e-8ce2-3ac0a0c79549"),
                            BirthDate = new DateTimeOffset(new DateTime(1976, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Hubei",
                            Email = "author2@xxx.com",
                            Name = "Author 2"
                        },
                        new
                        {
                            Id = new Guid("8406b13e-a793-4b12-84cb-7fe2a694b9aa"),
                            BirthDate = new DateTimeOffset(new DateTime(1973, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Hubei",
                            Email = "author3@xxx.com",
                            Name = "Author 3"
                        },
                        new
                        {
                            Id = new Guid("74556abd-1a6c-4d20-a8a7-271dd4393b2e"),
                            BirthDate = new DateTimeOffset(new DateTime(1978, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Shandong",
                            Email = "author4@xxx.com",
                            Name = "Author 4"
                        },
                        new
                        {
                            Id = new Guid("1029db57-c15c-4c0c-80a0-c811b7995cb4"),
                            BirthDate = new DateTimeOffset(new DateTime(1973, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Beijing",
                            Email = "author5@xxx.com",
                            Name = "Author 5"
                        },
                        new
                        {
                            Id = new Guid("0f978cf6-df6d-47a9-8ef2-d2723cc29cc8"),
                            BirthDate = new DateTimeOffset(new DateTime(1981, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Beijing",
                            Email = "author6@xxx.com",
                            Name = "Author 6"
                        },
                        new
                        {
                            Id = new Guid("10ee3976-d672-4411-ae1c-3267baa940eb"),
                            BirthDate = new DateTimeOffset(new DateTime(1954, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Shandong",
                            Email = "author7@xxx.com",
                            Name = "Author 7"
                        },
                        new
                        {
                            Id = new Guid("2633a79c-9f4a-48d5-ae5a-70945fb8583c"),
                            BirthDate = new DateTimeOffset(new DateTime(1981, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)),
                            BirthPlace = "Shandong",
                            Email = "author8@xxx.com",
                            Name = "Author 8"
                        });
                });

            modelBuilder.Entity("Library.API.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("Pages")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7d8ebda9-2634-4c0f-9469-0695d6132153"),
                            AuthorId = new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"),
                            Description = "Description of Book 1",
                            Pages = 281,
                            Title = "Book 1"
                        },
                        new
                        {
                            Id = new Guid("1ed47697-aa7d-48c2-aa39-305d0e13b3aa"),
                            AuthorId = new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"),
                            Description = "Description of Book 2",
                            Pages = 370,
                            Title = "Book 2"
                        },
                        new
                        {
                            Id = new Guid("5f82c852-375d-4926-a3b7-84b63fc1bfae"),
                            AuthorId = new Guid("7d04a48e-be4e-468e-8ce2-3ac0a0c79549"),
                            Description = "Description of Book 3",
                            Pages = 229,
                            Title = "Book 3"
                        },
                        new
                        {
                            Id = new Guid("418a5b20-460b-4604-be17-2b0809e19acd"),
                            AuthorId = new Guid("7d04a48e-be4e-468e-8ce2-3ac0a0c79549"),
                            Description = "Description of Book 4",
                            Pages = 440,
                            Title = "Book 4"
                        });
                });

            modelBuilder.Entity("Library.API.Entities.Commodity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("FirstClassify")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("PriceFixing")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Repertory")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("SecondClassify")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SelfCode")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Standard")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Tag")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Unit")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Commodities");
                });

            modelBuilder.Entity("Library.API.Entities.CommodityTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("CommodityTag");
                });

            modelBuilder.Entity("Library.API.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("GroupCode")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("UpGroupCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Library.API.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Library.API.Entities.SelfCommodity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("FirstClassify")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<decimal>("PriceFixing")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Repertory")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("SecondClassify")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SelfCode")
                        .HasColumnType("longtext");

                    b.Property<string>("Standard")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Tag")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Unit")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("SelfCommodity");
                });

            modelBuilder.Entity("Library.API.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Library.API.Entities.Book", b =>
                {
                    b.HasOne("Library.API.Entities.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Library.API.Entities.SelfCommodity", b =>
                {
                    b.HasOne("Library.API.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Library.API.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Library.API.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Library.API.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Library.API.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.API.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Library.API.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Library.API.Entities.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}