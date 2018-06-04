using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace Innofactor.EfCoreJsonValueConverter {

  public static class ModelBuilderExtensions {

    public static void AddJsonFields(this ModelBuilder modelBuilder) {

      foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
        foreach (var property in entityType.GetProperties()) {
          var attributes = property.PropertyInfo.GetCustomAttributes(typeof(JsonFieldAttribute), false);
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
