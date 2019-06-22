using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Innofactor.EfCoreJsonValueConverter.Test.Components
{
  public static class TestHelpers
  {
    public static ConventionSet GetDefaultConventions() {

      var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
          .AddDbContext<DbContext>((p, o) =>
            o.UseInMemoryDatabase(Guid.NewGuid().ToString())
              .UseInternalServiceProvider(p))
          .BuildServiceProvider();

      using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
        using (var context = serviceScope.ServiceProvider.GetService<DbContext>()) {
          return ConventionSet.CreateConventionSet(context);
        }
      }
    }
  }
}
