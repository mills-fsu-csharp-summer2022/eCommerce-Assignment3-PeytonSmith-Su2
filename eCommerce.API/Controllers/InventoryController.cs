using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;
using eCommerce.API.EC;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return new InventoryEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product product)
        {
            new InventoryEC().AddOrUpdate(product);
            return product;
        }

        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            new InventoryEC().Delete(id);
            return id;
        }
        [HttpPost("AddProductsToInventory")]
        public List<Product> AddProductsToInventory(List<Product> productList)
        {
            return new InventoryEC().AddProductsToInventory(productList);
        }

    }
}
