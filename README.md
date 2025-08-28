# Guardian

[![NuGet](https://img.shields.io/nuget/v/Guardian.svg)](https://www.nuget.org/packages/Guardian/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-6.0%20%7C%207.0%20%7C%208.0-512BD4)](https://dotnet.microsoft.com/)

Guardian is a lightweight, high-performance library providing guard clauses for validating method parameters and ensuring defensive programming practices in .NET applications.

## Features

- **Comprehensive Validation**: Guards against null, empty, default values, out-of-range values, and invalid formats
- **Type-Safe**: Full support for generics and nullable reference types
- **Performance Optimized**: Minimal overhead with inline methods and zero allocations for successful validations
- **Developer Friendly**: Intuitive API with IntelliSense support and detailed XML documentation
- **Framework Support**: Targets .NET 6.0, 7.0, 8.0, and .NET Standard 2.0/2.1
- **Modern C# Features**: Uses CallerArgumentExpression for automatic parameter name capture

## Installation

Install Guardian via NuGet:

```bash
dotnet add package Guardian
```

Or via Package Manager Console:

```powershell
Install-Package Guardian
```

## Quick Start

```csharp
using Guardian;

public class Product
{
    public string Name { get; }
    public decimal Price { get; }
    public int StockQuantity { get; }

    public Product(string name, decimal price, int stockQuantity)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Price = Guard.Against.NegativeOrZero(price);
        StockQuantity = Guard.Against.Negative(stockQuantity);
    }
}
```

## Available Guard Clauses

### Null Checks
- `Null<T>()` - Throws if value is null
- `NullOrEmpty()` - Throws if string/collection is null or empty
- `NullOrWhiteSpace()` - Throws if string is null, empty, or whitespace

### Default Value Checks
- `Default<T>()` - Throws if value equals default(T)

### Numeric Range Checks
- `Negative()` - Throws if value is negative
- `Zero()` - Throws if value is zero
- `NegativeOrZero()` - Throws if value is negative or zero
- `Positive()` - Throws if value is positive
- `OutOfRange()` - Throws if value is outside specified range
- `GreaterThan()` - Throws if value is greater than maximum
- `GreaterThanOrEqualTo()` - Throws if value is greater than or equal to maximum
- `LessThan()` - Throws if value is less than minimum
- `LessThanOrEqualTo()` - Throws if value is less than or equal to minimum

### String Validation
- `InvalidFormat()` - Throws if string doesn't match regex pattern
- `InvalidLength()` - Throws if string length is outside specified range

### Enum Validation
- `NotInEnum()` - Throws if value is not a defined enum value

### Collection Validation
- `NullOrEmpty()` - Throws if collection is null or empty

### Custom Validation
- `Condition()` - Throws if condition is false
- `NotOneOf()` - Throws if value is not in allowed values list

## Usage Examples

### Basic Parameter Validation

```csharp
public void ProcessOrder(Guid orderId, string customerEmail, decimal amount)
{
    Guard.Against.Default(orderId);
    Guard.Against.InvalidFormat(customerEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    Guard.Against.OutOfRange(amount, 0.01m, 10000m);
    
    // Process the order...
}
```

### Constructor Validation

```csharp
public class Customer
{
    public Guid Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Email { get; }

    public Customer(Guid id, string name, int age, string email)
    {
        Id = Guard.Against.Default(id);
        Name = Guard.Against.InvalidLength(name, 2, 100);
        Age = Guard.Against.OutOfRange(age, 18, 120);
        Email = Guard.Against.InvalidFormat(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
```

### Custom Business Rules

```csharp
public void TransferFunds(decimal amount, Account fromAccount, Account toAccount)
{
    Guard.Against.NegativeOrZero(amount);
    Guard.Against.Null(fromAccount);
    Guard.Against.Null(toAccount);
    Guard.Against.Condition(
        fromAccount.Balance >= amount,
        nameof(amount),
        $"Insufficient funds. Available: {fromAccount.Balance}, Requested: {amount}"
    );
    
    // Perform transfer...
}
```

### Enum Validation

```csharp
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public void UpdateOrderStatus(OrderStatus status)
{
    Guard.Against.NotInEnum(status);
    
    // Update status...
}
```

### Collection Validation

```csharp
public void ProcessItems(List<Item> items)
{
    Guard.Against.NullOrEmpty(items);
    
    foreach (var item in items)
    {
        Guard.Against.Null(item);
        // Process item...
    }
}
```

### Method Chaining

```csharp
public class Product
{
    private string _name;
    private decimal _price;

    public void Update(string name, decimal price)
    {
        _name = Guard.Against.NullOrWhiteSpace(name);
        _name = Guard.Against.InvalidLength(_name, 3, 50);
        
        _price = Guard.Against.NegativeOrZero(price);
        _price = Guard.Against.GreaterThan(_price, 99999.99m);
    }
}
```

### Custom Error Messages

```csharp
public void SetDiscount(decimal discountPercentage)
{
    Guard.Against.OutOfRange(
        discountPercentage, 
        0, 
        100,
        message: "Discount percentage must be between 0 and 100"
    );
    
    // Apply discount...
}
```

## Performance Considerations

Guardian is designed for minimal performance impact:

- All guard methods are optimized for the success path
- No allocations when validation passes
- Inline method hints for better JIT optimization
- Generic constraints prevent boxing of value types

## Best Practices

1. **Use at Method Entry Points**: Place guards at the beginning of methods to fail fast
2. **Be Specific**: Use the most specific guard clause for better error messages
3. **Custom Messages**: Provide custom messages for domain-specific validations
4. **Combine Guards**: Use multiple guards for comprehensive validation
5. **Consistent Usage**: Apply guards consistently across your codebase

## Error Handling

Guardian throws appropriate exceptions based on the validation type:

- `ArgumentNullException` - For null values
- `ArgumentException` - For invalid values, formats, or conditions
- `ArgumentOutOfRangeException` - For values outside acceptable ranges

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/guardian/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/guardian/discussions)
- **Documentation**: [Wiki](https://github.com/yourusername/guardian/wiki)

## Acknowledgments

Guardian is inspired by popular guard clause libraries and defensive programming practices in the .NET ecosystem.