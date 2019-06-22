
namespace Innofactor.EfCoreJsonValueConverter.Test.Entities {
  public class Office {
    public int Id { get; set; }

    public string CustomerServicePhone { get; set; }

    [JsonField]
    public Address Address { get; set; }

    public override string ToString() => $"{Id} {Address?.Street}";
  }

}