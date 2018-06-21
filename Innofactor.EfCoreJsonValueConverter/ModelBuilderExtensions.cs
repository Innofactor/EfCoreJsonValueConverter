using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace Innofactor.EfCoreJsonValueConverter {

  /// <summary>
  /// Extensions for <see cref="ModelBuilder"/>.
  /// </summary>
  public static class ModelBuilderExtensions {

    /// <summary>
    /// Add fields marked with <see cref="JsonFieldAttribute"/> to be converted using <see cref="JsonValueConverter{T}"/>.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance. Cannot be null.</param>
    /// <remarks>
    /// Adapted from https://www.tabsoverspaces.com/233708-using-value-converter-for-custom-encryption-of-field-on-entity-framework-core-2-1
    /// </remarks>
    public static void AddJsonFields(this ModelBuilder modelBuilder) {

      foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
        foreach (var property in entityType.GetProperties().Where(p => p?.PropertyInfo != null)) {
          var attributes = property.PropertyInfo?.GetCustomAttributes(typeof(JsonFieldAttribute), false);
          if (attributes.Any()) {
            var modelType = property.PropertyInfo.PropertyType;
            var converterType = typeof(JsonValueConverter<>).MakeGenericType(modelType);
            var converter = (ValueConverter)Activator.CreateInstance(converterType, new object[] { null });
            property.SetValueConverter(converter);
          }
        }
      }

    }

  }

}
