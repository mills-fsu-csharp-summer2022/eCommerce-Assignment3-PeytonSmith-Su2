using eCommerce.API.Database;
using Library.eCommerce.Models;
using System.Collections.Generic;

namespace eCommerce.API.EC
{
    public class CartEC
    {
        public List<string> Get()
        {
            return Filebase.Current.GetListOfCarts();
        }
        public List<Product>? GetProductFromCart(string name)
        {
            //if (FakeDatabase.Carts.ContainsKey(name))
            //{
            //    return FakeDatabase.Carts[name];
            //}
            //return null;
            return Filebase.Current.GetCart(name);
        }

        public int Delete(string name, int UID)
        {
            //var prodToDelete = FakeDatabase.Carts[name].FirstOrDefault(i => i.UID == UID);

            //if (prodToDelete != null)
            //{
            //    FakeDatabase.Carts[name].Remove(prodToDelete);
            //}

            //return prodToDelete ?? new Product();

            return Filebase.Current.DeleteCart(name, UID);
        }

        public Product AddOrUpdate(string name, Product p)
        {
            //if (p.Id <= 0)
            //{
            //    // If the database doesn't have the cart name 
            //    if (!FakeDatabase.Carts.ContainsKey(name))
            //    {
            //        FakeDatabase.Carts.Add(name, new List<Product>() { p });
            //    }
            //    else
            //    {
            //        FakeDatabase.Carts[name].Add(p);
            //    }
            //    p.Id = FakeDatabase.NextIdCart(name);
            //}
            //else
            //{
            //    var prodToReplace = FakeDatabase.Carts[name].FirstOrDefault(i => i.Id == p.Id);
            //    if (prodToReplace != null)
            //    {
            //        FakeDatabase.Carts[name].Remove(prodToReplace);
            //    }
            //    FakeDatabase.Carts[name].Add(p);
            //}
            //return p;
            return Filebase.Current.AddOrUpdateCart(name,p);
        }
        public string Add(string name)
        {
            //FakeDatabase.Carts[name] = new List<Product>();
            //return name;
            return Filebase.Current.AddCart(name);
        }
        public string DeleteCart(string name)
        {
            //// Check if database contains name of cart, if so delete
            //if (FakeDatabase.Carts.ContainsKey(name))
            //{
            //    FakeDatabase.Carts.Remove(name);
            //}
            //return name;
            return Filebase.Current.DeleteCartEntirely(name);
        }
        public List<Product> AddProductsToCart(string name, List<Product> productList)
        {
            //if(productList != null)
            //{
            //    FakeDatabase.Carts[name] = productList;
            //}
            //return productList ?? new List<Product>();

            if (productList != null)
            {
                return Filebase.Current.AddProductsToCart(name, productList);
            }
            return new List<Product>();
        }
        public Product UpdateMetaData(string name, Product p)
        {
            var prodToReplace = Filebase.Current.ReturnProductFoundInCart(name, p);
            //var prodToReplace = FakeDatabase.Carts[name].FirstOrDefault(i => i.UID == p.UID);
            if (prodToReplace != null)
            {
                // Update everything but the quantity/weight, used whenever updating info in inventory
                prodToReplace.Bogo = p.Bogo;
                prodToReplace.Name = p.Name;
                prodToReplace.Description = p.Description;
                prodToReplace.Price = p.Price;
                prodToReplace.UID = p.UID;
            }
            Filebase.Current.AddOrUpdateCart(name, prodToReplace);
            return prodToReplace;
        }
    }
}
