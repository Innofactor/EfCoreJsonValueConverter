using System.Text.Json;

namespace Innofactor.EfCoreJsonValueConverter;

internal static class JsonHelper {

  public static T Deserialize<T>(string json) where T : class
    => string.IsNullOrWhiteSpace(json)
      ? null
      : JsonSerializer.Deserialize<T>(json, EfCoreJsonValueConverterConfiguration.DefaultJsonSerializerOptions);

  public static string Serialize<T>(T obj) where T : class
    => obj == null
      ? null
      : JsonSerializer.Serialize(obj, EfCoreJsonValueConverterConfiguration.DefaultJsonSerializerOptions);

}
