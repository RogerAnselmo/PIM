using Microsoft.EntityFrameworkCore;
using PIM.Api.Models;

namespace PIM.Api.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions options) : base(options) { }
        public DbSet<SystemUser> SystemUsers { get; set; }
    }
}
