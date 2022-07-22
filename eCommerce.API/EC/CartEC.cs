using eCommerce.API.Database;
using Library.eCommerce.Models;
using System.Collections.Generic;

namespace eCommerce.API.EC
{
    public class CartEC
    {
        public Dictionary<string, List<Product>>.KeyCollection Get()
        {
            return FakeDatabase.Carts.Keys;
        }
        public List<Product>? GetProductFromCart(string name)
        {
            if (FakeDatabase.Carts.ContainsKey(name))
            {
                return FakeDatabase.Carts[name];
            }
            return null;
        }

        public Product Delete(string name, int id)
        {
            var prodToDelete = FakeDatabase.Carts[name].FirstOrDefault(i => i.Id == id);

            if (prodToDelete != null)
            {
                FakeDatabase.Carts[name].Remove(prodToDelete);
            }

            return prodToDelete ?? new Product();
        }

        public Product AddOrUpdate(string name, Product p)
        {
            if (p.Id <= 0)
            {
                // If the database doesn't have the cart name 
                if (!FakeDatabase.Carts.ContainsKey(name))
                {
                    FakeDatabase.Carts.Add(name, new List<Product>() { p });
                }
                else
                {
                    FakeDatabase.Carts[name].Add(p);
                }
                p.Id = FakeDatabase.NextIdCart(name);
            }
            else
            {
                var prodToReplace = FakeDatabase.Carts[name].FirstOrDefault(i => i.Id == p.Id);
                if (prodToReplace != null)
                {
                    FakeDatabase.Carts[name].Remove(prodToReplace);
                }
                FakeDatabase.Carts[name].Add(p);
            }
            return p;
        }
        public string Add(string name)
        {
            FakeDatabase.Carts[name] = new List<Product>();
            return name;
        }
        public string DeleteCart(string name)
        {
            // Check if database contains name of cart, if so delete
            if (FakeDatabase.Carts.ContainsKey(name))
            {
                FakeDatabase.Carts.Remove(name);
            }
            return name;
        }
        public List<Product> AddProductsToCart(string name, List<Product> productList)
        {
            if(name != "NoNameCart" && FakeDatabase.Carts.ContainsKey("NoNameCart")){
                FakeDatabase.Carts.Remove("NoNameCart");
            }
            if(productList != null)
            {
                FakeDatabase.Carts[name] = productList;
            }
            return productList ?? new List<Product>();
        }
    }
}
