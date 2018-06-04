# JSON value converter for Entity Framework Core 2.1x

Serializes object properties in database as JSON blobs using [Entity Framework Core value converter](https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions).

## Usage example

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

    base.OnModelCreating(builder);

    builder.Entity<Customer>().Property(m => m.Address);
    builder.AddJsonFields();        
    
  }
```

