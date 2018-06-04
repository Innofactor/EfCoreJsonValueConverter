using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Innofactor.EfCoreJsonValueConverter.Test {

  [TestClass]
  public class ModelBuilderExtensionsTests {

    [TestMethod]
    public void AddJsonFields() {

      var modelBuilder = new ModelBuilder(new ConventionSet());
      modelBuilder.Entity<Parent>().Property(m => m.Child);
      modelBuilder.AddJsonFields();        

      var model = modelBuilder.Model;
      var modelType = model.FindEntityType(typeof(Parent));
      var modelProperty = modelType.FindProperty(nameof(Parent.Child));

      Assert.IsInstanceOfType(modelProperty.GetValueConverter(), typeof(JsonValueConverter<Child>), "Value converter was applied");

    }

    public class Parent {
      [JsonField]
      public Child Child { get; set; }
    }

    public class Child {
      public int Id {get; set; }
    }

  }

}
