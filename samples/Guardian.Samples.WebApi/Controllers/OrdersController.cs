using Noundry.Guardian;
using Noundry.Guardian.Samples.WebApi.Models;
using Noundry.Guardian.Samples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Noundry.Guardian.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = Guard.Against.Null(orderService);
            _logger = Guard.Against.Null(logger);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found");
                }
                
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid order ID");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] CreateOrderRequest request)
        {
            try
            {
                Guard.Against.Null(request);
                
                var order = await _orderService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Order data out of range");
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null order data");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid order data");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<Order>> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                Guard.Against.Null(request);
                
                var order = await _orderService.UpdateStatusAsync(id, request.Status);
                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found");
                }
                
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid status update");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                
                var deleted = await _orderService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound($"Order with ID {id} not found");
                }
                
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid order ID");
                return BadRequest(ex.Message);
            }
        }
    }
}