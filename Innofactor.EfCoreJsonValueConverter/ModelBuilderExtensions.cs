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
      return propertyInfo != null && propertyInfo.CustomAttributes.Any(a => a.AttributeType == typeof(JsonFieldAttribute));
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

        foreach (var clrProperty in entityType.ClrType.GetProperties().Where(HasJsonAttribute)) {
          var property = modelBuilder.Entity(entityType.ClrType).Property(clrProperty.PropertyType, clrProperty.Name);
          var modelType = clrProperty.PropertyType;

          var converterType = typeof(JsonValueConverter<>).MakeGenericType(modelType);
          var converter = (ValueConverter)Activator.CreateInstance(converterType, new object[] { null });
          property.Metadata.SetValueConverter(converter);

          var valueComparer = typeof(JsonValueComparer<>).MakeGenericType(modelType);
          property.Metadata.SetValueComparer((ValueComparer)Activator.CreateInstance(valueComparer, new object[0]));
        }
      }
    }

  }
}
