# JSON value converter for Entity Framework Core 3.0+

![status badge](https://innofactor-agile.visualstudio.com/_apis/public/build/definitions/8f49bcda-8276-4721-8f2e-aa1f54924edf/19/badge)

Serializes object properties in database as JSON blobs using [Entity Framework Core value converters](https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions).

## Usage example

Installing:
```
PM> Install-Package Innofactor.EfCoreJsonValueConverter
```

Annotate model properties with the ```JsonField``` attribute:
```csharp
public class Customer {
  [JsonField]
  public Address Address { get; set; }
}

public class Address {
  // ...
}
```

This will persist Address property in database as JSON.

For DbContext:

```csharp
  protected override void OnModelCreating(ModelBuilder builder) {

    builder.Entity<Customer>();
    builder.AddJsonFields();        
    
  }
```

Alternatively, individual fields can be mapped with the HasJsonValueConversion method:
```csharp
  protected override void OnModelCreating(ModelBuilder builder) {

    builder.Entity<Customer>()
      .Property(m => m.Address)
      .HasJsonValueConversion();
    
  }
```

## Note on performance

EF Core requires that tracked objects implement deep cloning and comparison.
This library includes default fallback implementations for cloning and comparing, 
but for large objects that default implementation may be inefficient. 
To improve performance, it is recommended to manually implement ICloneable and IEquatable<T> interfaces for complex
JSON serialized objects. This library will then make use of those interfaces.

```csharp
public class Address : ICloneable, IEquatable<Address> {
  // ...
}
```