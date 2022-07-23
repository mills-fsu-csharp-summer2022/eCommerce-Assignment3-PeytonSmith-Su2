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
        // Gets a list of all cart names
        [HttpGet]
        public List<string?> Get()
        {
            return new CartEC().Get();
        }
        // Adds a cart to the database
        [HttpPost("AddCart/{name}")]
        public string Add(string name)
        {
            return new CartEC().Add(name);
        }
        // Deletes a cart from the database
        [HttpGet("DeleteCart/{name}")]
        public string DeleteCart(string name)
        {
            return new CartEC().DeleteCart(name);
        }
        // Grabs all products from a specific cart
        [HttpGet("{name}")]
        public List<Product> GetProductFromCart(string name)
        {
            return new CartEC().GetProductFromCart(name) ?? new List<Product>();
        }
        // Add or updates a product from a specific cart
        [HttpPost("AddOrUpdate/{name}")]
        public Product AddOrUpdate(string name, Product product)
        {
            return new CartEC().AddOrUpdate(name, product);
        }
        // Updates the meta data of a product in a cart
        // Used when changing meta data on the inventory side, the changes will persist through all carts
        [HttpPost("UpdateMetaData/{name}")]
        public Product UpdateMetaData(string name, Product product)
        {
            new CartEC().UpdateMetaData(name, product);
            return product;
        }
        // Deletes a product from a specific cart
        [HttpGet("Delete/{name}/{uid}")]
        public int Delete(string name, int uid)
        {
            new CartEC().Delete(name, uid);
            return uid;
        }
        // Used to synchronize the productList on the client to the server
        [HttpPost("AddProductsToCart/{name}")]
        public List<Product> AddProductsToCart(string name, List<Product> productList)
        {
            return new CartEC().AddProductsToCart(name, productList);
        }

    }
}
