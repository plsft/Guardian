using Guardian;

namespace Guardian.Samples.WebApi.Models
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Toys,
        Sports
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductCategory Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Product(Guid id, string name, string description, decimal price, int stockQuantity, ProductCategory category)
        {
            Id = Guard.Against.DefaultStruct(id);
            Name = Guard.Against.NullOrWhiteSpace(name);
            Name = Guard.Against.InvalidLength(Name, 3, 100);
            Description = Guard.Against.NullOrEmpty(description);
            Description = Guard.Against.InvalidLength(Description, 10, 500);
            Price = Guard.Against.NegativeOrZero(price);
            Price = Guard.Against.GreaterThan(Price, 999999.99m);
            StockQuantity = Guard.Against.Negative(stockQuantity);
            Category = Guard.Against.NotInEnum(category);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice)
        {
            Price = Guard.Against.NegativeOrZero(newPrice);
            Price = Guard.Against.GreaterThan(Price, 999999.99m);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStock(int quantity)
        {
            StockQuantity = Guard.Against.Negative(quantity);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddStock(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity);
            StockQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveStock(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity);
            Guard.Against.Condition(
                quantity <= StockQuantity,
                nameof(quantity),
                $"Cannot remove {quantity} items. Only {StockQuantity} available."
            );
            
            StockQuantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductCategory Category { get; set; }
    }

    public class UpdateProductPriceRequest
    {
        public decimal Price { get; set; }
    }

    public class UpdateProductStockRequest
    {
        public int Quantity { get; set; }
    }
}