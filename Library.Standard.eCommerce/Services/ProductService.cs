using Library.eCommerce.Models;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class ProductService
    {
        private string persistPath
    = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private List<Product> productList;

        public List<Product> Products
        {
            get
            {
                //var productsJson = new WebRequestHandler().Get("http://localhost:5127/Product");
                return productList;
            }
            set
            {
                productList = value;
            }
        }

        public int NextId
        {
            get
            {
                if (!Products.Any())
                {
                    return 1;
                }

                return Products.Select(t => t.Id).Max() + 1;
            }
        }

        private static ProductService current;
        private static ProductService current2;

        public static ProductService Current
        {
            get
            {
                if (current == null)
                {
                    current = new ProductService();
                }
                //SetUpInventory();
                return current;
            }
        }
        public static ProductService Current2
        {
            get
            {
                if (current2 == null)
                {
                    current2 = new ProductService();
                }
                //SetUpCart();
                return current2;
            }
        }
        //private static void SetUpCart()
        //{
        //    var productsJson = new WebRequestHandler().Get("http://localhost:5127/Product/Cart").Result;
        //    Current.Products = JsonConvert.DeserializeObject<List<Product>>(productsJson);
        //}
        //private static void SetUpInventory()
        //{
        //    var productsJson = new WebRequestHandler().Get("http://localhost:5127/Product/Inventory").Result;
        //    productList = JsonConvert.DeserializeObject<List<Product>>(productsJson);
        //}

        // Should only ever be used when adding to inventory
        public void SetUID(Product product)
        {
            product.UID = NextId;
        }

        private Product ExistingProductInList { get; set; }
        public bool CheckProductInList(Product product)
        {
            var products = Products;
            for (var i = 0; i < products.Count; i++)
            {
                // Check if UID of a product matches
                if (products[i].UID == product.UID)
                {
                    ExistingProductInList = products[i];
                    return true;
                }

            }
            return false;
        }
        public Product ReturnExistingProductInList()
        {
            return ExistingProductInList;
        }

        public ProductService()
        {
            productList = new List<Product>();
        }

        public void AddOrUpdate(Product product)
        {
            //if (product.Id <= 0)
            //{
            //    product.Id = NextId;
            //    Products.Add(product);
            //}

            if (product is ProductByQuantity)
            {
                var response = new WebRequestHandler().Post("http://localhost:5127/ProductByQuantity/AddOrUpdate/Inventory", product).Result;
                var newProduct = JsonConvert.DeserializeObject<ProductByQuantity>(response);

                var oldVersion = productList.FirstOrDefault(i => i.Id == newProduct.Id);
                if (oldVersion != null)
                {
                    var index = productList.IndexOf(oldVersion);
                    productList.RemoveAt(index);
                    productList.Insert(index, newProduct);
                }
                else
                {
                    productList.Add(newProduct);
                }

            }
            else if (product is ProductByWeight)
            {
                var response = new WebRequestHandler().Post("http://localhost:5127/ProductByWeight/AddOrUpdate", product).Result;
                var newProduct = JsonConvert.DeserializeObject<ProductByWeight>(response);

                var oldVersion = productList.FirstOrDefault(i => i.Id == newProduct.Id);
                if (oldVersion != null)
                {
                    var index = productList.IndexOf(oldVersion);
                    productList.RemoveAt(index);
                    productList.Insert(index, newProduct);
                }
                else
                {
                    productList.Add(newProduct);
                }
            }
        }

        public void Delete(int uid)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5127/Product/Delete/{uid}");
            var productToDelete = productList.FirstOrDefault(t => t.UID == uid);
            if (productToDelete == null)
            {
                return;
            }
            productList.Remove(productToDelete);
        }

        public void Load(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var productsJson = File.ReadAllText(fileName);
            productList = JsonConvert.DeserializeObject<List<Product>>
                (productsJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();

        }

        public void Save(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }
            var productsJson = JsonConvert.SerializeObject(productList
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, productsJson);
        }
        private string _currentcart { get; set; }
        public string CurrentCart
        {
            get { return _currentcart; }
            set { _currentcart = value; }
        }

    }
}
