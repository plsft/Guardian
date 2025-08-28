using Guardian;

namespace Guardian.Samples.WebApi.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public Order(
            Guid id, 
            string customerName, 
            string customerEmail, 
            string shippingAddress, 
            List<OrderItem> items)
        {
            Id = Guard.Against.DefaultStruct(id);
            CustomerName = Guard.Against.NullOrWhiteSpace(customerName);
            CustomerName = Guard.Against.InvalidLength(CustomerName, 2, 100);
            CustomerEmail = Guard.Against.InvalidFormat(
                customerEmail,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
            ShippingAddress = Guard.Against.NullOrWhiteSpace(shippingAddress);
            ShippingAddress = Guard.Against.InvalidLength(ShippingAddress, 10, 500);
            
            var validatedItems = Guard.Against.NullOrEmpty(items);
            _items.AddRange(validatedItems);
            
            TotalAmount = CalculateTotal();
            Guard.Against.NegativeOrZero(TotalAmount);
            Guard.Against.GreaterThan(TotalAmount, 1000000m);
            
            Status = OrderStatus.Pending;
            OrderDate = DateTime.UtcNow;
        }

        private decimal CalculateTotal()
        {
            return _items.Sum(item => item.Price * item.Quantity);
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = Guard.Against.NotInEnum(newStatus);
            
            // Business rule validations
            if (Status == OrderStatus.Cancelled && newStatus != OrderStatus.Cancelled)
            {
                Guard.Against.Condition(
                    false,
                    nameof(newStatus),
                    "Cannot change status of a cancelled order"
                );
            }
            
            if (Status == OrderStatus.Delivered)
            {
                Guard.Against.Condition(
                    false,
                    nameof(newStatus),
                    "Cannot change status of a delivered order"
                );
            }
        }

        public void AddItem(OrderItem item)
        {
            Guard.Against.Null(item);
            Guard.Against.Condition(
                !_items.Any(i => i.ProductId == item.ProductId),
                nameof(item),
                "Product already exists in the order"
            );
            
            _items.Add(item);
            TotalAmount = CalculateTotal();
        }
    }

    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public OrderItem(Guid productId, string productName, decimal price, int quantity)
        {
            ProductId = Guard.Against.DefaultStruct(productId);
            ProductName = Guard.Against.NullOrWhiteSpace(productName);
            Price = Guard.Against.NegativeOrZero(price);
            Quantity = Guard.Against.NegativeOrZero(quantity);
        }
    }

    public class CreateOrderRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }

    public class CreateOrderItemRequest
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}