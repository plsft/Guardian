using Noundry.Guardian;
using Noundry.Guardian.Samples.WebApi.Models;
using System.Collections.Concurrent;

namespace Noundry.Guardian.Samples.WebApi.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(CreateOrderRequest request);
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> UpdateStatusAsync(Guid id, OrderStatus status);
        Task<bool> DeleteAsync(Guid id);
    }

    public class OrderService : IOrderService
    {
        private readonly ConcurrentDictionary<Guid, Order> _orders = new();

        public Task<Order> CreateAsync(CreateOrderRequest request)
        {
            Guard.Against.Null(request);
            Guard.Against.NullOrEmpty(request.Items);
            
            var orderItems = request.Items.Select(item => new OrderItem(
                item.ProductId,
                item.ProductName,
                item.Price,
                item.Quantity
            )).ToList();
            
            var order = new Order(
                Guid.NewGuid(),
                request.CustomerName,
                request.CustomerEmail,
                request.ShippingAddress,
                orderItems
            );
            
            _orders.TryAdd(order.Id, order);
            return Task.FromResult(order);
        }

        public Task<Order?> GetByIdAsync(Guid id)
        {
            Guard.Against.DefaultStruct(id);
            
            _orders.TryGetValue(id, out var order);
            return Task.FromResult(order);
        }

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            return Task.FromResult(_orders.Values.AsEnumerable());
        }

        public Task<Order?> UpdateStatusAsync(Guid id, OrderStatus status)
        {
            Guard.Against.DefaultStruct(id);
            Guard.Against.NotInEnum(status);
            
            if (_orders.TryGetValue(id, out var order))
            {
                order.UpdateStatus(status);
                return Task.FromResult<Order?>(order);
            }
            
            return Task.FromResult<Order?>(null);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            Guard.Against.DefaultStruct(id);
            
            return Task.FromResult(_orders.TryRemove(id, out _));
        }
    }
}