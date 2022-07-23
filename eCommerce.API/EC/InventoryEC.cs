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
            //var prodToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.UID == id);

            //if(prodToDelete != null)
            //{
            //    FakeDatabase.Inventory.Remove(prodToDelete);
            //}

            //return prodToDelete ?? new Product();

            return Filebase.Current.DeleteInventory(id);
        }

        public Product AddOrUpdate(Product p)
        {
            //if(p.Id <= 0)
            //{
            //    p.Id = FakeDatabase.NextIdInventory();
            //    FakeDatabase.Inventory.Add(p);
            //}
            //else
            //{
            //    var productToReplace = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == p.Id);
            //    if(productToReplace != null)
            //    {
            //        FakeDatabase.Inventory.Remove(productToReplace);
            //    }
            //    FakeDatabase.Inventory.Add(p);
            //}            
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
