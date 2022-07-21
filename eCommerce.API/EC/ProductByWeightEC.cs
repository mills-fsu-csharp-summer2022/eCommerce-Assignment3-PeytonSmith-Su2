using Library.eCommerce.Models;
using eCommerce.API.Database;

namespace eCommerce.API.EC
{
    public class ProductByWeightEC
    {
        public List<ProductByWeight> Get()
        {
            return FakeDatabase.ProductsByWeightInventory;
        }
    }
}
