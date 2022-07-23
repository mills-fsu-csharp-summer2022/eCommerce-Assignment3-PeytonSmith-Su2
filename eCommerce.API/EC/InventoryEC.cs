using eCommerce.API.Database;
using Library.eCommerce.Models;

namespace eCommerce.API.EC
{
    public class InventoryEC
    {
        public List<Product> Get()
        {
            return Filebase.Current.GetInventory();
        }

        public int Delete(int id)
        {
            return Filebase.Current.DeleteInventory(id);
        }

        public Product AddOrUpdate(Product p)
        {         
            return Filebase.Current.AddOrUpdateInventory(p);
        }
        public List<Product> AddProductsToInventory(List<Product> productList)
        {
            if (productList != null)
            {
                return Filebase.Current.AddProductsToInventory(productList);
            }
            return new List<Product>();
        }
    }
}
