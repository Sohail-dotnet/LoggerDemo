using LoggerDemo.Model;
using Microsoft.AspNetCore.Mvc;

namespace LoggerDemo.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private static List<Product> _products = new List<Product>
        {
            new Product { Id =1, Name = "Laptop", Price = 50000 },
            new Product { Id =2, Name = "Mobile", Price = 20000 },
            new Product { Id =3, Name = "Tablet", Price = 30000 }
        };

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Fetching all Products");
            return Ok(_products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Product Id");
            }
            _logger.LogInformation("Fetching Product with Id: {Id}", id);
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning($"Product with Id: {id} not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name))
            {
                _logger.LogError("Attempted To Add Invalid Product Data");
                return BadRequest("Invalid Product Data");
            }

            try
            {
                product.Id = _products.Max(p => p.Id) + 1;
                _products.Add(product);
                _logger.LogInformation($"Successfully Added New Product:{product}  ");
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new product");
                return StatusCode(500, "Internal server error");
            }   
        }
    }
}
