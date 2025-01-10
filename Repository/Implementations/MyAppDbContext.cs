using Microsoft.EntityFrameworkCore;
using MyApp.Cross.Models;

namespace MyApp.Repository
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
    }
}
