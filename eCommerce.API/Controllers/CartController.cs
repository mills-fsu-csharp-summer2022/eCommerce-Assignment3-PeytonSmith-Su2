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
        public List<string?> Get()
        {
            return new CartEC().Get();
        }

        [HttpPost("AddCart/{name}")]
        public string Add(string name)
        {
            return new CartEC().Add(name);
        }
        [HttpGet("DeleteCart/{name}")]
        public string DeleteCart(string name)
        {
            return new CartEC().DeleteCart(name);
        }

        [HttpGet("{name}")]
        public List<Product> GetProductFromCart(string name)
        {
            return new CartEC().GetProductFromCart(name) ?? new List<Product>();
        }

        [HttpPost("AddOrUpdate/{name}")]
        public Product AddOrUpdate(string name, Product product)
        {
            return new CartEC().AddOrUpdate(name, product);
        }
        [HttpPost("UpdateMetaData/{name}")]
        public Product UpdateMetaData(string name, Product product)
        {
            new CartEC().UpdateMetaData(name, product);
            return product;
        }

        [HttpGet("Delete/{name}/{uid}")]
        public int Delete(string name, int uid)
        {
            new CartEC().Delete(name, uid);
            return uid;
        }
        [HttpPost("AddProductsToCart/{name}")]
        public List<Product> AddProductsToCart(string name, List<Product> productList)
        {
            return new CartEC().AddProductsToCart(name, productList);
        }

    }
}
