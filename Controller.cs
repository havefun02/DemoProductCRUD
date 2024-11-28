using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace DemoCRUD
{
    [ApiController]
    [Route("/api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, IMapper mapper, ILogger<ProductsController> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
                    .FromSqlRaw("CALL sp_GetAllProducts")
                    .ToListAsync();

                var response = _mapper.Map<List<ProductResponse>>(products);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products.");
                return StatusCode(500, "An unexpected error occurred while retrieving products.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            try
            {
                var param = new MySqlParameter("@Id", id);
                var product = await _context.Products
                    .FromSqlRaw("CALL sp_GetProductById(@Id)", param).ToListAsync();
                if (product == null)
                {
                    return NotFound();
                }
                if (product.Count > 1)
                {
                    return StatusCode(500, "An unexpected error occurred while retrieving the product.");
                }

                var response = _mapper.Map<ProductResponse>(product.FirstOrDefault());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while retrieving the product.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> AddProduct([FromBody] CreateProductRequest product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var targetProduct = _mapper.Map<Product>(product);
                _context.Products.Add(targetProduct);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<ProductResponse>(targetProduct);
                return CreatedAtAction(nameof(GetProductById), new { id = result.ProductId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product.");
                return StatusCode(500, "An unexpected error occurred while adding the product.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var targetProduct = _mapper.Map<Product>(product);
                targetProduct.ProductId = id;
                _context.Entry(targetProduct).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.ProductId == id))
                    return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while updating the product.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while deleting the product.");
            }
        }
    }
}
