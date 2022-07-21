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
    public class CartService
    {
        private string persistPath
    = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private static List<Product> productList;

        public List<Product> Products
        {
            get
            {
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

        private static CartService current;
        public static CartService Current
        {
            get
            {
                if (current == null)
                {
                    current = new CartService();
                }
                return current;
            }
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

        public CartService()
        {
            var productsCartJson = new WebRequestHandler().Get("http://localhost:5127/Cart").Result;
            productList = JsonConvert.DeserializeObject<List<Product>>(productsCartJson);
        }

        public void AddOrUpdate(Product product)
        {
            var response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddOrUpdate/{CurrentCart}", product).Result;
            var newProduct = JsonConvert.DeserializeObject<Product>(response);

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

        public void Delete(int uid)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/Delete/{CurrentCart}/{uid}");
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
