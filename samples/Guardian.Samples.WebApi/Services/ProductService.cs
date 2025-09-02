using Noundry.Guardian;
using Noundry.Guardian.Samples.WebApi.Models;
using System.Collections.Concurrent;

namespace Noundry.Guardian.Samples.WebApi.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(CreateProductRequest request);
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> UpdatePriceAsync(Guid id, decimal newPrice);
        Task<Product?> UpdateStockAsync(Guid id, int quantity);
        Task<bool> DeleteAsync(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly ConcurrentDictionary<Guid, Product> _products = new();

        public ProductService()
        {
            // Seed with sample data
            SeedProducts();
        }

        public Task<Product> CreateAsync(CreateProductRequest request)
        {
            Guard.Against.Null(request);
            
            var product = new Product(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity,
                request.Category
            );
            
            _products.TryAdd(product.Id, product);
            return Task.FromResult(product);
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            Guard.Against.DefaultStruct(id);
            
            _products.TryGetValue(id, out var product);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_products.Values.AsEnumerable());
        }

        public Task<Product?> UpdatePriceAsync(Guid id, decimal newPrice)
        {
            Guard.Against.DefaultStruct(id);
            Guard.Against.NegativeOrZero(newPrice);
            
            if (_products.TryGetValue(id, out var product))
            {
                product.UpdatePrice(newPrice);
                return Task.FromResult<Product?>(product);
            }
            
            return Task.FromResult<Product?>(null);
        }

        public Task<Product?> UpdateStockAsync(Guid id, int quantity)
        {
            Guard.Against.DefaultStruct(id);
            Guard.Against.Negative(quantity);
            
            if (_products.TryGetValue(id, out var product))
            {
                product.UpdateStock(quantity);
                return Task.FromResult<Product?>(product);
            }
            
            return Task.FromResult<Product?>(null);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            Guard.Against.DefaultStruct(id);
            
            return Task.FromResult(_products.TryRemove(id, out _));
        }

        private void SeedProducts()
        {
            var products = new[]
            {
                new Product(
                    Guid.NewGuid(),
                    "Laptop Pro 15",
                    "High-performance laptop with 15-inch display, 16GB RAM, and 512GB SSD",
                    1299.99m,
                    25,
                    ProductCategory.Electronics
                ),
                new Product(
                    Guid.NewGuid(),
                    "Wireless Mouse",
                    "Ergonomic wireless mouse with precision tracking and long battery life",
                    29.99m,
                    150,
                    ProductCategory.Electronics
                ),
                new Product(
                    Guid.NewGuid(),
                    "Programming Book",
                    "Complete guide to modern software development practices and patterns",
                    49.99m,
                    75,
                    ProductCategory.Books
                ),
                new Product(
                    Guid.NewGuid(),
                    "Running Shoes",
                    "Professional running shoes with advanced cushioning technology",
                    119.99m,
                    50,
                    ProductCategory.Sports
                ),
                new Product(
                    Guid.NewGuid(),
                    "Coffee Maker",
                    "Automatic coffee maker with programmable timer and thermal carafe",
                    89.99m,
                    30,
                    ProductCategory.Electronics
                )
            };
            
            foreach (var product in products)
            {
                _products.TryAdd(product.Id, product);
            }
        }
    }
}