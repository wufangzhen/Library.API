using Library.API.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Entities
{
    public class LibraryDbContext : IdentityDbContext<User, Role, string>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Commodity> Commodities { get; set; }

        public DbSet<SelfCommodity> SelfCommodity { get; set; }

        public DbSet<CommodityTag> CommodityTag {  get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedData();
        }
    }
}