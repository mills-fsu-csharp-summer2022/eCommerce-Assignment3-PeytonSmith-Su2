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
        // Gets a list of all products in inventory
        [HttpGet]
        public List<Product> Get()
        {
            return new InventoryEC().Get();
        }
        // Add or updates a product in the database
        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product product)
        {
            new InventoryEC().AddOrUpdate(product);
            return product;
        }
        // Deletes a specific product in the inventory database
        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            new InventoryEC().Delete(id);
            return id;
        }
        // Used to synchronize the productList on the client to the server
        [HttpPost("AddProductsToInventory")]
        public List<Product> AddProductsToInventory(List<Product> productList)
        {
            return new InventoryEC().AddProductsToInventory(productList);
        }

    }
}
