using System;

namespace Innofactor.EfCoreJsonValueConverter {

  [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  public class JsonFieldAttribute : Attribute {}

}
