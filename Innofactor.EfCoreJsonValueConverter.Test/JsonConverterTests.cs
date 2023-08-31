using Microsoft.VisualStudio.TestTools.UnitTesting;
using Innofactor.EfCoreJsonValueConverter.Test.Entities;
using FluentAssertions;

namespace Innofactor.EfCoreJsonValueConverter.Test
{
  [TestClass]
  public class JsonConverterTests {

    [TestMethod]
    public void ApplyCustomJsonConverter() {
      var customer = new Customer { ProtectedAddress = new Address { Street = "Privet Drive 4" } };
      var converter = new JsonValueConverter<Customer>();

      var serialized = (string)converter.ConvertToProvider(customer);
      //Assert.IsTrue(serialized.Contains("Privet"), "Serialization applied custom converter");
      serialized.Should().ContainAll(
        "{",
        "protectedAddress",
        "street",
        customer.ProtectedAddress.Street,
        "}"
      );

      var deserialized = (Customer)converter.ConvertFromProvider(serialized);
      Assert.AreEqual("Privet Drive 4", deserialized.ProtectedAddress.Street, "Deserialization applied custom converter");

      deserialized.Should().BeEquivalentTo(customer);
    }
  }
}
