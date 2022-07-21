using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using eCommerce.API.Database;
using eCommerce.API.EC;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Dictionary<string, List<Product>>.KeyCollection Get()
        {
            return new CartEC().Get();
        }

        [HttpGet("{name}")]
        public List<Product> GetProductFromCart(string name)
        {
            return new CartEC().GetProductFromCart(name) ?? new List<Product>();
        }

        [HttpPost("AddOrUpdate/{name}")]
        public Product AddOrUpdate(string name, Product product)
        {
            new CartEC().AddOrUpdate(name, product);
            return product;
        }

        [HttpGet("Delete/{name}/{id}")]
        public int Delete(string name, int id)
        {
            new CartEC().Delete(name, id);
            return id;
        }

    }
}
