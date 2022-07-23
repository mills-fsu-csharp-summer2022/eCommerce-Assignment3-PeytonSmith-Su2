using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.eCommerce.Models;

namespace eCommerce.API.Database
{
    public class Filebase
    {
        private string _root;
        private string _inventoryRoot;
        private string _cartRoot;
        private static Filebase? _instance;

        public static List<Product> SampleInventoryItems = new List<Product>
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

        public static Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Filebase";
            _inventoryRoot = $"{_root}\\Inventory";
            _cartRoot = $"{_root}\\Carts";
            foreach (var product in SampleInventoryItems)
            {
                AddOrUpdateInventory(product);
            }
        }

        public Product AddOrUpdateInventory(Product product)
        {
            Directory.CreateDirectory(_inventoryRoot);
            if (product.Id <= 0)
            {
                product.Id = NextIdInventory();
            }
            //go to the right place
            string path;
            path = $"{_inventoryRoot}/{product.Id}.json";

            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(product));

            //return the item, which now has an id
            return product;
        }

        public Product AddOrUpdateCart(string name, Product product)
        {
            AddCart(name);
            if (product.Id <= 0)
            {
                product.Id = NextIdCart(name);
            }
            //go to the right place
            string path;
            path = $"{_cartRoot}/{name}/{product.Id}.json";

            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(product));

            //return the item, which now has an id
            return product;
        }
        public List<Product> GetInventory()
        {
            var root = new DirectoryInfo(_inventoryRoot);
            var _products = new List<Product>();
            foreach (var productFile in root.GetFiles())
            {
                var product = JsonConvert.DeserializeObject<Product>(File.ReadAllText(productFile.FullName));
                if(product != null)
                {
                    _products.Add(product);
                }
            }
            return _products;
        }
        public List<Product> GetCart(string name)
        {
            var root = new DirectoryInfo($"{_cartRoot}/{name}");
            var _products = new List<Product>();
            foreach (var productFile in root.GetFiles())
            {
                var product = JsonConvert.DeserializeObject<Product>(File.ReadAllText(productFile.FullName));
                if(product != null)
                {
                    _products.Add(product);
                }
            }
            return _products;
        }
        public List<string?> GetListOfCarts()
        {
            List<string?> listOfCarts = Directory.GetDirectories(_cartRoot)
                            .Select(Path.GetFileName)
                            .ToList();
            if(listOfCarts != null)
            {
                return listOfCarts;
            }
            return new List<string?>();
        }

        public int DeleteCart(string name, int id)
        {
            string path;
            path = $"{_cartRoot}/{name}/{id}.json";

            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }
            return id;
        }
        public int DeleteInventory(int id)
        {
            string path;
            path = $"{_inventoryRoot}/{id}.json";

            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }
            return id;
        }
        public string DeleteCartEntirely(string name)
        {
            string path;
            path = $"{_cartRoot}/{name}";
            Directory.Delete(path, true);
            return name;
        }
        public string AddCart(string name)
        {
            string path;
            path = $"{_cartRoot}/{name}";
            Directory.CreateDirectory(path);
            return name;
        }
        public List<Product> AddProductsToCart(string name, List<Product> productList)
        {
            foreach(var product in productList)
            {
                AddOrUpdateCart(name, product);
            }
            if(productList.Count == 0)
            {
                DeleteCartEntirely(name);
            }
            return productList;
        }
        public List<Product> AddProductsToInventory(List<Product> productList)
        {
            foreach (var product in productList)
            {
                AddOrUpdateInventory(product);
            }
            return productList;
        }
        public Product ReturnProductFoundInCart(string name, Product product)
        {
            var productList = GetCart(name);
            var productFound = productList.FirstOrDefault(i => i.UID == product.UID);
            if(productFound != null)
            {
                return productFound;
            }
            return new Product();
        }
        public int NextIdInventory()
        {
            if (InventoryEmpty())
            {
                return 1;
            }
            return GetInventory().Select(i => i.Id).Max() + 1;
        }
        public int NextIdCart(string name)
        {
            if (CartEmpty(name))
            {
                return 1;
            }
            return GetCart(name).Select(i => i.Id).Max() + 1;
        }
        public bool CartEmpty(string name)
        {
            var root = new DirectoryInfo($"{_cartRoot}/{name}");
            var count = root.GetFiles().Length;
            return count == 0;
        }
        public bool InventoryEmpty()
        {
            var root = new DirectoryInfo(_inventoryRoot);
            var count = root.GetFiles().Length;
            return count == 0;
        }
    }
}
