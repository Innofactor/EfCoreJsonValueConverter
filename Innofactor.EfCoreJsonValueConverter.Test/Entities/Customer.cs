using Innofactor.EfCoreJsonValueConverter.Test.Components;
using Newtonsoft.Json;

namespace Innofactor.EfCoreJsonValueConverter.Test.Entities
{
  public class Customer
  {
    public int Id { get; set; }

    [JsonField]
    public Address Address { get; set; }

    [JsonField]
    public AddressWithEquality Address2 { get; set; }

    [JsonField]
    [JsonConverter(typeof(ObscureAddressJsonConverter))]
    public Address ProtectedAddress { get; set; }
  }

  public class CustomerWithPlainField
  {
    private string name;
  }
}
