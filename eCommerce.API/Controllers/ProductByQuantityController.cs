using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;
using eCommerce.API.EC;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductByQuantityController : ControllerBase
    {
        private readonly ILogger<ProductByQuantityController> _logger;

        public ProductByQuantityController(ILogger<ProductByQuantityController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Inventory")]
        public List<ProductByQuantity> Get()
        {
            return new ProductByQuantityEC().Get();
        }

        [HttpPost("AddOrUpdate/Inventory")]
        public ProductByQuantity AddOrUpdate(ProductByQuantity product)
        {
            if (product.Id <= 0)
            {
                product.Id = FakeDatabase.NextIdInventory();
                FakeDatabase.ProductsByQuantityInventory.Add(product);
            }

            var productToUpdate = FakeDatabase.ProductsByQuantityInventory.FirstOrDefault(t => t.Id == product.Id);
            if (productToUpdate != null)
            {
                FakeDatabase.ProductsByQuantityInventory.Remove(productToUpdate);
                FakeDatabase.ProductsByQuantityInventory.Add(product);
            }

            return product;
        }

        [HttpGet("Delete/Inventory/{id}")]
        public int Delete(int id)
        {
            var productToDelete = FakeDatabase.ProductsInventory.FirstOrDefault(t => t.Id == id);
            if (productToDelete != null)
            {
                var product = productToDelete as ProductByQuantity;
                if (product != null)
                {
                    FakeDatabase.ProductsByQuantityInventory.Remove(product);
                }

            }

            return id;
        }
    }
}
