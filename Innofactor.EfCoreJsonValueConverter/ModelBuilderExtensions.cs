using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Reflection;

namespace Innofactor.EfCoreJsonValueConverter {

  /// <summary>
  /// Extensions for <see cref="ModelBuilder"/>.
  /// </summary>
  public static partial class ModelBuilderExtensions {

    private static bool HasJsonAttribute(PropertyInfo propertyInfo) {
      var attributes = propertyInfo?.GetCustomAttributes(typeof(JsonFieldAttribute), false);
      return attributes != null && attributes.Any();
    }

    /// <summary>
    /// Add fields marked with <see cref="JsonFieldAttribute"/> to be converted using <see cref="JsonValueConverter{T}"/>.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance. Cannot be null.</param>
    /// <remarks>
    /// Adapted from https://www.tabsoverspaces.com/233708-using-value-converter-for-custom-encryption-of-field-on-entity-framework-core-2-1
    /// </remarks>
    public static void AddJsonFields(this ModelBuilder modelBuilder) {

      if (modelBuilder == null)
        throw new ArgumentNullException(nameof(modelBuilder));

      foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {

        foreach (var property in entityType.GetProperties().Where(p => HasJsonAttribute(p.PropertyInfo))) {
          var modelType = property.PropertyInfo.PropertyType;
          var converterType = typeof(JsonValueConverter<>).MakeGenericType(modelType);
          var converter = (ValueConverter)Activator.CreateInstance(converterType, new object[] { null });
          property.SetValueConverter(converter);
          var valueComparer = typeof(JsonValueComparer<>).MakeGenericType(modelType);
          property.SetValueComparer((ValueComparer)Activator.CreateInstance(valueComparer, new object[0]));
        }

      }

    }

  }

}
