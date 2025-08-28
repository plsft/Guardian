using Guardian;
using System;
using System.Collections.Generic;

namespace Guardian.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Guardian Library Samples");
            System.Console.WriteLine("========================\n");

            RunProductSample();
            RunOrderSample();
            RunUserRegistrationSample();
            RunBankAccountSample();
            
            System.Console.WriteLine("\nAll samples completed successfully!");
        }

        static void RunProductSample()
        {
            System.Console.WriteLine("Product Sample:");
            System.Console.WriteLine("---------------");
            
            try
            {
                // Valid product creation
                var product = new Product(
                    Guid.NewGuid(),
                    "Gaming Laptop",
                    1299.99m,
                    15,
                    ProductStatus.Available
                );
                
                System.Console.WriteLine($"✓ Created product: {product.Name} - ${product.Price}");
                
                // Update price
                product.UpdatePrice(1199.99m);
                System.Console.WriteLine($"✓ Updated price to: ${product.Price}");
                
                // Add stock
                product.AddStock(10);
                System.Console.WriteLine($"✓ Added stock. Current quantity: {product.StockQuantity}");
                
                // Try invalid operations
                try
                {
                    var invalidProduct = new Product(
                        Guid.Empty,  // Will throw
                        "Test",
                        100m,
                        5,
                        ProductStatus.Available
                    );
                }
                catch (ArgumentException ex)
                {
                    System.Console.WriteLine($"✓ Caught expected error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"✗ Unexpected error: {ex.Message}");
            }
            
            System.Console.WriteLine();
        }

        static void RunOrderSample()
        {
            System.Console.WriteLine("Order Sample:");
            System.Console.WriteLine("-------------");
            
            try
            {
                var productIds = new List<Guid> 
                { 
                    Guid.NewGuid(), 
                    Guid.NewGuid(), 
                    Guid.NewGuid() 
                };
                
                var order = new Order(
                    Guid.NewGuid(),
                    "customer@example.com",
                    299.99m,
                    productIds
                );
                
                System.Console.WriteLine($"✓ Created order with {order.ProductIds.Count} products");
                System.Console.WriteLine($"✓ Customer email: {order.CustomerEmail}");
                System.Console.WriteLine($"✓ Total amount: ${order.TotalAmount}");
                
                // Add another product
                order.AddProductId(Guid.NewGuid());
                System.Console.WriteLine($"✓ Added product. Total products: {order.ProductIds.Count}");
                
                // Try invalid email
                try
                {
                    var invalidOrder = new Order(
                        Guid.NewGuid(),
                        "invalid-email",  // Will throw
                        100m,
                        new List<Guid> { Guid.NewGuid() }
                    );
                }
                catch (ArgumentException ex)
                {
                    System.Console.WriteLine($"✓ Caught expected error for invalid email: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"✗ Unexpected error: {ex.Message}");
            }
            
            System.Console.WriteLine();
        }

        static void RunUserRegistrationSample()
        {
            System.Console.WriteLine("User Registration Sample:");
            System.Console.WriteLine("------------------------");
            
            try
            {
                var user = new UserRegistration(
                    "JohnDoe",
                    "john.doe@example.com",
                    25,
                    "SecurePass123!"
                );
                
                System.Console.WriteLine($"✓ Registered user: {user.Username}");
                System.Console.WriteLine($"✓ Email: {user.Email}");
                System.Console.WriteLine($"✓ Age: {user.Age}");
                
                // Try various invalid inputs
                TestInvalidRegistration("", "test@example.com", 25, "Pass123!", "empty username");
                TestInvalidRegistration("Jo", "test@example.com", 25, "Pass123!", "username too short");
                TestInvalidRegistration("ValidUser", "invalid", 25, "Pass123!", "invalid email");
                TestInvalidRegistration("ValidUser", "test@example.com", 17, "Pass123!", "age too young");
                TestInvalidRegistration("ValidUser", "test@example.com", 25, "short", "password too short");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"✗ Unexpected error: {ex.Message}");
            }
            
            System.Console.WriteLine();
        }

        static void TestInvalidRegistration(string username, string email, int age, string password, string expectedError)
        {
            try
            {
                var user = new UserRegistration(username, email, age, password);
                System.Console.WriteLine($"✗ Should have thrown for: {expectedError}");
            }
            catch (Exception)
            {
                System.Console.WriteLine($"✓ Correctly rejected: {expectedError}");
            }
        }

        static void RunBankAccountSample()
        {
            System.Console.WriteLine("Bank Account Sample:");
            System.Console.WriteLine("-------------------");
            
            try
            {
                var account = new BankAccount(
                    "1234567890",
                    "John Doe",
                    1000m
                );
                
                System.Console.WriteLine($"✓ Created account: {account.AccountNumber}");
                System.Console.WriteLine($"✓ Account holder: {account.AccountHolder}");
                System.Console.WriteLine($"✓ Initial balance: ${account.Balance}");
                
                // Deposit
                account.Deposit(500m);
                System.Console.WriteLine($"✓ Deposited $500. New balance: ${account.Balance}");
                
                // Withdraw
                account.Withdraw(200m);
                System.Console.WriteLine($"✓ Withdrew $200. New balance: ${account.Balance}");
                
                // Try overdraft
                try
                {
                    account.Withdraw(2000m);
                    System.Console.WriteLine("✗ Should have thrown for overdraft");
                }
                catch (ArgumentException ex)
                {
                    System.Console.WriteLine($"✓ Correctly prevented overdraft: {ex.Message}");
                }
                
                // Transfer
                var targetAccount = new BankAccount("0987654321", "Jane Doe", 500m);
                account.Transfer(targetAccount, 300m);
                System.Console.WriteLine($"✓ Transferred $300. Source balance: ${account.Balance}, Target balance: ${targetAccount.Balance}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"✗ Unexpected error: {ex.Message}");
            }
            
            System.Console.WriteLine();
        }
    }

    // Sample domain models using Guardian

    public enum ProductStatus
    {
        Available,
        OutOfStock,
        Discontinued
    }

    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public ProductStatus Status { get; private set; }

        public Product(Guid id, string name, decimal price, int stockQuantity, ProductStatus status)
        {
            Id = Guard.Against.DefaultStruct(id);
            Name = Guard.Against.NullOrWhiteSpace(name);
            Name = Guard.Against.InvalidLength(Name, 2, 100);
            Price = Guard.Against.NegativeOrZero(price);
            StockQuantity = Guard.Against.Negative(stockQuantity);
            Status = Guard.Against.NotInEnum(status);
        }

        public void UpdatePrice(decimal newPrice)
        {
            Price = Guard.Against.NegativeOrZero(newPrice);
        }

        public void AddStock(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity);
            StockQuantity += quantity;
        }

        public void RemoveStock(int quantity)
        {
            Guard.Against.NegativeOrZero(quantity);
            Guard.Against.Condition(
                quantity <= StockQuantity,
                nameof(quantity),
                $"Cannot remove {quantity} items when only {StockQuantity} are in stock."
            );
            
            StockQuantity -= quantity;
        }
    }

    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerEmail { get; private set; }
        public decimal TotalAmount { get; private set; }
        private readonly List<Guid> _productIds = new List<Guid>();
        public IReadOnlyCollection<Guid> ProductIds => _productIds.AsReadOnly();

        public Order(Guid id, string customerEmail, decimal totalAmount, List<Guid> productIds)
        {
            Id = Guard.Against.DefaultStruct(id);
            CustomerEmail = Guard.Against.InvalidFormat(
                customerEmail,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
            TotalAmount = Guard.Against.OutOfRange(totalAmount, 0.01m, 100000m);
            _productIds.AddRange(Guard.Against.NullOrEmpty(productIds));
        }

        public void AddProductId(Guid productId)
        {
            Guard.Against.DefaultStruct(productId);
            Guard.Against.Condition(
                !_productIds.Contains(productId),
                nameof(productId),
                "Product is already in the order."
            );
            
            _productIds.Add(productId);
        }
    }

    public class UserRegistration
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public int Age { get; private set; }
        public string PasswordHash { get; private set; }

        public UserRegistration(string username, string email, int age, string password)
        {
            Username = Guard.Against.NullOrWhiteSpace(username);
            Username = Guard.Against.InvalidLength(Username, 3, 20);
            
            Email = Guard.Against.InvalidFormat(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
            
            Age = Guard.Against.OutOfRange(age, 18, 120);
            
            var validatedPassword = Guard.Against.InvalidLength(password, 8, 100);
            PasswordHash = HashPassword(validatedPassword);
        }

        private string HashPassword(string password)
        {
            // Simplified for example - use proper hashing in production
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public class BankAccount
    {
        public string AccountNumber { get; private set; }
        public string AccountHolder { get; private set; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountNumber, string accountHolder, decimal initialBalance)
        {
            AccountNumber = Guard.Against.InvalidFormat(
                accountNumber,
                @"^\d{10}$"
            );
            AccountHolder = Guard.Against.NullOrWhiteSpace(accountHolder);
            AccountHolder = Guard.Against.InvalidLength(AccountHolder, 2, 100);
            Balance = Guard.Against.Negative(initialBalance);
        }

        public void Deposit(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);
            Guard.Against.GreaterThan(amount, 1000000m);
            
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);
            Guard.Against.Condition(
                amount <= Balance,
                nameof(amount),
                $"Insufficient funds. Available: ${Balance}, Requested: ${amount}"
            );
            
            Balance -= amount;
        }

        public void Transfer(BankAccount targetAccount, decimal amount)
        {
            Guard.Against.Null(targetAccount);
            Guard.Against.NegativeOrZero(amount);
            Guard.Against.Condition(
                amount <= Balance,
                nameof(amount),
                "Insufficient funds for transfer"
            );
            
            Withdraw(amount);
            targetAccount.Deposit(amount);
        }
    }
}