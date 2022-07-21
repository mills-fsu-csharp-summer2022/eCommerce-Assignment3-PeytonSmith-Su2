using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Inventory")]
        public List<Product> GetInventory()
        {
            return FakeDatabase.ProductsInventory;
        }

        [HttpGet("Cart")]
        public List<Product> GetCart()
        {
            return FakeDatabase.ProductsCart;
        }

    }
}
