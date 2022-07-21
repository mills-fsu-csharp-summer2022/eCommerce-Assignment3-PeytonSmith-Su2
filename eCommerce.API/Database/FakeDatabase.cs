using Library.eCommerce.Models;

namespace eCommerce.API.Database
{
    public static class FakeDatabase
    {
        public static List<Product> Inventory= new List<Product>
        {
            new ProductByQuantity { Name = "Pizza", Description = "Cheese", Price = 5, Bogo = false, Quantity = 100, Id = 1, UID = 1, QuantityTest = 1},
            new ProductByQuantity { Name = "Bicycle", Description = "Red", Price = 100, Bogo = false, Quantity = 15, Id = 2, UID = 2, QuantityTest = 1},
            new ProductByQuantity { Name = "Toy", Description = "For kids", Price = 3, Bogo = true, Quantity = 500, Id = 3, UID = 3, QuantityTest = 1},
            new ProductByQuantity { Name = "Pizza", Description = "Pepperoni", Price = 6, Bogo = false, Quantity = 200, Id = 4, UID = 4, QuantityTest = 1},
            new ProductByWeight { Name = "Dog food", Description = "Protein", Price = 4, Bogo = true, Weight = 40, Id = 5, UID = 5, WeightTest = 1},
            new ProductByWeight { Name = "Meat", Description = "Steak", Price = 20, Bogo = false, Weight = 55.5, Id = 6, UID = 6, WeightTest = 1},
            new ProductByWeight { Name = "Meat", Description = "Ground Beef", Price = 2, Bogo = false, Weight = 200, Id = 7, UID = 7, WeightTest = 1},
            new ProductByWeight { Name = "Dog food", Description = "Snacks", Price = 2.5, Bogo = false, Weight = 400, Id = 8, UID = 8, WeightTest = 1}
        };
        private static Dictionary<string, List<Product>> carts = new Dictionary<string, List<Product>>();
        public static Dictionary<string, List<Product>> Carts
        {
            get
            {
                return carts;
            }
            set
            {
                carts = value;
            }
        }

        public static int NextIdInventory()
        {
            return Inventory.Select(i => i.Id).Max() + 1;
        }
        public static int NextIdCart(string name)
        {
            return Carts[name].Select(i => i.Id).Max() + 1;
        }
    }
}
