using eCommerce.API.Database;
using Library.eCommerce.Models;

namespace eCommerce.API.EC
{
    public class InventoryEC
    {
        public List<Product> Get()
        {
            return FakeDatabase.Inventory;
        }

        public Product Delete(int id)
        {
            var prodToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.UID == id);

            if(prodToDelete != null)
            {
                FakeDatabase.Inventory.Remove(prodToDelete);
            }

            return prodToDelete ?? new Product();
        }

        public Product AddOrUpdate(Product p)
        {
            if(p.Id <= 0)
            {
                p.Id = FakeDatabase.NextIdInventory();
                FakeDatabase.Inventory.Add(p);
            }
            else
            {
                var productToReplace = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == p.Id);
                if(productToReplace != null)
                {
                    FakeDatabase.Inventory.Remove(productToReplace);
                }
                FakeDatabase.Inventory.Add(p);
            }
            return p;
        }
    }
}
