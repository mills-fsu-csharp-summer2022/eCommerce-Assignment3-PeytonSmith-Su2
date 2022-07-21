using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;
using eCommerce.API.EC;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductByWeightController : ControllerBase
    {
        private readonly ILogger<ProductByWeightController> _logger;

        public ProductByWeightController(ILogger<ProductByWeightController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Inventory")]
        public List<ProductByWeight> Get()
        {
            return new ProductByWeightEC().Get();
        }

        [HttpPost("AddOrUpdate/Inventory")]
        public ProductByWeight AddOrUpdate(ProductByWeight product)
        {
            if (product.Id <= 0)
            {
                product.Id = FakeDatabase.NextIdInventory();
                FakeDatabase.ProductsByWeightInventory.Add(product);
            }

            var productToUpdate = FakeDatabase.ProductsByWeightInventory.FirstOrDefault(t => t.Id == product.Id);
            if (productToUpdate != null)
            {
                FakeDatabase.ProductsByWeightInventory.Remove(productToUpdate);
                FakeDatabase.ProductsByWeightInventory.Add(product);
            }

            return product;
        }

        [HttpGet("Delete/Inventory/{id}")]
        public int Delete(int id)
        {
            var productToDelete = FakeDatabase.ProductsInventory.FirstOrDefault(t => t.Id == id);
            if (productToDelete != null)
            {
                var product = productToDelete as ProductByWeight;
                if (product != null)
                {
                    FakeDatabase.ProductsByWeightInventory.Remove(product);
                }

            }

            return id;
        }
    }
}
