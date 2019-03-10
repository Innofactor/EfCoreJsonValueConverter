using System;
using System.Linq;
using Innofactor.EfCoreJsonValueConverter.Test.Entities;
using Newtonsoft.Json;

namespace Innofactor.EfCoreJsonValueConverter.Test.Components {
  public class ObscureAddressJsonConverter : JsonConverter {
    private const int ObscureKey = 101;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      var address = (Address)value;
      writer.WriteValue(Xorify(address.Street, ObscureKey));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.Value == null) return null;

      var address = new Address {Street = Xorify(reader.Value.ToString(), ObscureKey)};
      return address;
    }

    public override bool CanConvert(Type objectType) {
      return objectType == typeof(Address);
    }

    /// <summary>
    /// Simple encryp / decrypt with xor + key
    /// </summary>
    private static string Xorify(string value, int key) {
      return value.Aggregate("", (prev, current) => prev + (char)(current ^ key));
    }
  }
}
