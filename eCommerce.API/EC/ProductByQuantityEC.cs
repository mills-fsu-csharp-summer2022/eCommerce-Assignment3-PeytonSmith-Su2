using Library.eCommerce.Models;
using eCommerce.API.Database;

namespace eCommerce.API.EC
{
    public class ProductByQuantityEC
    {
        public List<ProductByQuantity> Get()
        {
            return FakeDatabase.ProductsByQuantityInventory;
        }
    }
}
