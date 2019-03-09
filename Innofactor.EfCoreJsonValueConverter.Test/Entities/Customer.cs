namespace Innofactor.EfCoreJsonValueConverter.Test.Entities
{
  public class Customer
  {
    public int Id { get; set; }

    [JsonField]
    public Address Address { get; set; }

    [JsonField]
    public AddressWithEquality Address2 { get; set; }

  }

  public class CustomerWithPlainField
  {
    private string name;
  }
}
