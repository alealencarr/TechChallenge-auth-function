using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TechChallenge_auth_function.Domain.Entities;

namespace TechChallenge_auth_function.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
