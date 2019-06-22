using Innofactor.EfCoreJsonValueConverter.Test.Entities;
using Microsoft.EntityFrameworkCore;

namespace Innofactor.EfCoreJsonValueConverter.Test.Components
{
  public class TestDbContext : DbContext
  {
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseInMemoryDatabase("Test");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Customer>(c => {
        c.Property(e => e.Address).HasJsonValueConversion();
        c.Property(e => e.Address2).HasJsonValueConversion();
        c.Property(e => e.ProtectedAddress).HasJsonValueConversion();
        c.Ignore(e => e.Office);
      });
    }
  }
}
