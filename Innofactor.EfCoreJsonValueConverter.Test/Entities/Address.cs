using System;
using System.Collections.Generic;

namespace Innofactor.EfCoreJsonValueConverter.Test.Entities {
  public class Address {
    public string Street { get; set; }

    public override string ToString() => Street;
  }

  public class AddressWithEquality : IEquatable<AddressWithEquality>, ICloneable {

    public string Street { get; set; }

    public object Clone() {
      return new AddressWithEquality { Street = Street };
    }

    public bool Equals(AddressWithEquality obj) => obj != null && Street == obj.Street;

    public override int GetHashCode() {
      return 294059534 + EqualityComparer<string>.Default.GetHashCode(Street);
    }

    public override string ToString() => Street;

  }

}