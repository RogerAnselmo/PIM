using Microsoft.EntityFrameworkCore;
using PIM.Api.Core.Models;

namespace PIM.Api.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions options) : base(options) { }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
    }
}
