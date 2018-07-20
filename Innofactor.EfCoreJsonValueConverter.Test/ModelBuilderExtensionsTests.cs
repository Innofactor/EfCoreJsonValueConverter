using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Innofactor.EfCoreJsonValueConverter.Test {

  /// <summary>
  /// Tests for <see cref="ModelBuilderExtensions"/>.
  /// </summary>
  [TestClass]
  public class ModelBuilderExtensionsTests {

    private readonly ModelBuilder modelBuilder;

    public ModelBuilderExtensionsTests() {
      modelBuilder = new ModelBuilder(new ConventionSet());
    }

    [TestMethod]
    public void AddJsonFields() {

      modelBuilder.Entity<Customer>().Property(m => m.Address);
      modelBuilder.AddJsonFields();        

      var model = modelBuilder.Model;
      var modelType = model.FindEntityType(typeof(Customer));
      var modelProperty = modelType.FindProperty(nameof(Customer.Address));

      Assert.IsInstanceOfType(modelProperty.GetValueConverter(), typeof(JsonValueConverter<Address>), "Value converter was applied");

    }

    [TestMethod]
    public void AddJsonFields_ShadowProperty() {

      modelBuilder.Entity<CustomerWithPlainField>().Property<string>("Name").HasField("name");
      modelBuilder.AddJsonFields();        

      var model = modelBuilder.Model;
      var modelType = model.FindEntityType(typeof(CustomerWithPlainField));
      var modelProperty = modelType.FindProperty("Name");

      Assert.IsNotNull(modelProperty, "Plain field was added");

    }

    [TestMethod]
    public void DetectChanges_PlainObject() {

      using (var db  = new TestDbContext()) {

        var customer = new Customer { Address = new Address { Street = "Street 1" }};
        db.Add(customer);
        db.SaveChanges();

        // Note: changing tracking does not work without reloading. This might be an issue with EF Core itself?
        db.Entry(customer).State = EntityState.Detached;
        customer = db.Customers.Find(customer.Id);

        Assert.AreEqual(EntityState.Unchanged, db.Entry(customer).State, "Precondition: entity is marked as unchanged");

        customer.Address.Street = "Street 2";

        db.ChangeTracker.DetectChanges();

        Assert.AreEqual(EntityState.Modified, db.Entry(customer).State, "Entity is marked as modified");
        Assert.IsTrue(db.Entry(customer).Property(m => m.Address).IsModified, "Property is marked as modified");

      }
    }

    [TestMethod]
    public void DetectChanges_WithCustomEquality() {

      using (var db  = new TestDbContext()) {

        var customer = new Customer { Address2 = new AddressWithEquality { Street = "Street 1" }};
        db.Add(customer);
        db.SaveChanges();

        db.Entry(customer).State = EntityState.Detached;
        customer = db.Customers.Find(customer.Id);

        Assert.AreEqual(EntityState.Unchanged, db.Entry(customer).State, "Precondition: entity is marked as unchanged");

        customer.Address2.Street = "Street 2";

        db.ChangeTracker.DetectChanges();

        Assert.AreEqual(EntityState.Modified, db.Entry(customer).State, "Entity is marked as modified");
        Assert.IsTrue(db.Entry(customer).Property(m => m.Address2).IsModified, "Property is marked as modified");

      }

    }

    public class Customer {
      public int Id { get; set; }
      [JsonField]
      public Address Address { get; set; }
      [JsonField]
      public AddressWithEquality Address2 { get; set; }
    }

    public class CustomerWithPlainField {
      private string name;
    }

    public class Address {

      public string Street { get; set; }

      public override string ToString() => Street;

    }

    public class AddressWithEquality : IEquatable<AddressWithEquality>, ICloneable {

      public string Street { get; set; }

      public object Clone() {
        return new AddressWithEquality { Street = Street };
      }

      public bool Equals(AddressWithEquality obj) => obj != null && Street == obj.Street;

      public override int GetHashCode() {
        return 294059534 + EqualityComparer<string>.Default.GetHashCode(Street);
      }

      public override string ToString() => Street;

    }

    public class TestDbContext : DbContext {
      public DbSet<Customer> Customers { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase("Test");
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Customer>(c => {
          c.Property(e => e.Address).HasJsonValueConversion();
          c.Property(e => e.Address2).HasJsonValueConversion();
        });
      }
    }

  }

}
