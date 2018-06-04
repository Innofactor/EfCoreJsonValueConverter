using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Innofactor.EfCoreJsonValueConverter.Test {

  /// <summary>
  /// Tests for <see cref="ModelBuilderExtensions"/>.
  /// </summary>
  [TestClass]
  public class ModelBuilderExtensionsTests {

    [TestMethod]
    public void AddJsonFields() {

      var modelBuilder = new ModelBuilder(new ConventionSet());
      modelBuilder.Entity<Customer>().Property(m => m.Address);
      modelBuilder.AddJsonFields();        

      var model = modelBuilder.Model;
      var modelType = model.FindEntityType(typeof(Customer));
      var modelProperty = modelType.FindProperty(nameof(Customer.Address));

      Assert.IsInstanceOfType(modelProperty.GetValueConverter(), typeof(JsonValueConverter<Address>), "Value converter was applied");

    }

    public class Customer {
      [JsonField]
      public Address Address { get; set; }
    }

    public class Address {
      public int Id {get; set; }
    }

  }

}
