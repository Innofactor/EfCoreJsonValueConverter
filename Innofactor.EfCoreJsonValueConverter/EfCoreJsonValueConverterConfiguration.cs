using System.Text.Json;
using System.Text.Json.Serialization;

namespace Innofactor.EfCoreJsonValueConverter;

/// <summary>A static configuration class to expose the serializer settings used for the JSON value converter.</summary>
public static class EfCoreJsonValueConverterConfiguration
{
    /// <summary>Gets the default JSON serializer options.</summary>
    public static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
        }
    };
}