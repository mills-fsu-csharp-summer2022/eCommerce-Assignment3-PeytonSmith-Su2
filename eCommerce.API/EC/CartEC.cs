using eCommerce.API.Database;
using Library.eCommerce.Models;
using System.Collections.Generic;

namespace eCommerce.API.EC
{
    public class CartEC
    {
        public List<string?> Get()
        {
            return Filebase.Current.GetListOfCarts();
        }
        public List<Product>? GetProductFromCart(string name)
        {
            return Filebase.Current.GetCart(name);
        }

        public int Delete(string name, int UID)
        {
            return Filebase.Current.DeleteCart(name, UID);
        }

        public Product AddOrUpdate(string name, Product p)
        {
            return Filebase.Current.AddOrUpdateCart(name,p);
        }
        public string Add(string name)
        {
            return Filebase.Current.AddCart(name);
        }
        public string DeleteCart(string name)
        {
            return Filebase.Current.DeleteCartEntirely(name);
        }
        public List<Product> AddProductsToCart(string name, List<Product> productList)
        {

            if (productList != null)
            {
                return Filebase.Current.AddProductsToCart(name, productList);
            }
            return new List<Product>();
        }
        public Product UpdateMetaData(string name, Product p)
        {
            var prodToReplace = Filebase.Current.ReturnProductFoundInCart(name, p);
            if (prodToReplace != null)
            {
                // Update everything but the quantity/weight, used whenever updating info in inventory
                prodToReplace.Bogo = p.Bogo;
                prodToReplace.Name = p.Name;
                prodToReplace.Description = p.Description;
                prodToReplace.Price = p.Price;
                prodToReplace.UID = p.UID;
                return Filebase.Current.AddOrUpdateCart(name, prodToReplace);
            }
            return new Product();
        }
    }
}
