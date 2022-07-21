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
                p.Id = FakeDatabase.NextIdCart(name);
                FakeDatabase.Carts[name].Add(p);
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
    }
}
