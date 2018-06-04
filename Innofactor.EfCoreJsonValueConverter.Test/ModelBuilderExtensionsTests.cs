using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    public class Customer {
      [JsonField]
      public Address Address { get; set; }
    }

    public class CustomerWithPlainField {
      private string name;
    }

    public class Address {
      public int Id {get; set; }
    }

  }

}
