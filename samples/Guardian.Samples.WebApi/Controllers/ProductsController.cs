using Guardian;
using Guardian.Samples.WebApi.Models;
using Guardian.Samples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Guardian.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = Guard.Against.Null(productService);
            _logger = Guard.Against.Null(logger);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid product ID");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] CreateProductRequest request)
        {
            try
            {
                Guard.Against.Null(request);
                
                var product = await _productService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Product data out of range");
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Null product data");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid product data");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/price")]
        public async Task<ActionResult<Product>> UpdatePrice(Guid id, [FromBody] UpdateProductPriceRequest request)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                Guard.Against.Null(request);
                
                var product = await _productService.UpdatePriceAsync(id, request.Price);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                return Ok(product);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Price out of range");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid price update");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/stock")]
        public async Task<ActionResult<Product>> UpdateStock(Guid id, [FromBody] UpdateProductStockRequest request)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                Guard.Against.Null(request);
                
                var product = await _productService.UpdateStockAsync(id, request.Quantity);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                return Ok(product);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "Stock quantity out of range");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid stock update");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                Guard.Against.DefaultStruct(id);
                
                var deleted = await _productService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid product ID");
                return BadRequest(ex.Message);
            }
        }
    }
}