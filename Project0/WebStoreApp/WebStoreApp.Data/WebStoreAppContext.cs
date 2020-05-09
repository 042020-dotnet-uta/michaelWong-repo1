using Microsoft.EntityFrameworkCore;
using WebStoreApp.Domain;

namespace WebStoreApp.Data
{
    public class WebStoreAppContext : DbContext
    {
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<LoginInfo> LoginInfos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public WebStoreAppContext(DbContextOptions<WebStoreAppContext> options)
            : base(options)
        {}
    }
}
