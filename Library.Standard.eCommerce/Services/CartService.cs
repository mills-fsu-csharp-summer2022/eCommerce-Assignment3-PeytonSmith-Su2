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
            //var productsCartJson = new WebRequestHandler().Get("http://localhost:5127/Cart").Result;
            //productList = JsonConvert.DeserializeObject<List<Product>>(productsCartJson);
            productList = new List<Product>();
        }

        public void AddOrUpdate(Product product)
        {
            if (CurrentCart == null)
            {
                CurrentCart = "NoNameCart";
            }
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
            //if (string.IsNullOrEmpty(fileName))
            //{
            //    fileName = $"{persistPath}\\SaveData.json";
            //}
            //else
            //{
            //    fileName = $"{persistPath}\\{fileName}.json";
            //}

            //var productsJson = File.ReadAllText(fileName);
            //productList = JsonConvert.DeserializeObject<List<Product>>
            //    (productsJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
            //    ?? new List<Product>();

            var productsJson = new WebRequestHandler().Get($"http://localhost:5127/Cart/{fileName}").Result;
            productList = JsonConvert.DeserializeObject<List<Product>>(productsJson);
            CurrentCart = fileName;
        }
        public void DeleteCart(string fileName = null)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/DeleteCart/{CurrentCart}");
        }
        public void Save(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "NoNameCart";
            }
            else if(CurrentCart != null)
            {
                cartNames.Remove(CurrentCart);
                cartNames.Add(fileName);
            }
            // Delete the current cart in the database, and then add it with the new name 
            var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/DeleteCart/{CurrentCart}");
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddCart/{fileName}", fileName);
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddProductsToCart/{fileName}", productList);
            _currentcart = fileName;
            //if (string.IsNullOrEmpty(fileName))
            //{
            //    fileName = $"{persistPath}\\SaveData.json";
            //}
            //else
            //{
            //    fileName = $"{persistPath}\\{fileName}.json";
            //}
            //_currentcart = fileName;
            //var productsJson = JsonConvert.SerializeObject(productList
            //    , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            //File.WriteAllText(fileName, productsJson);
        }
        public void NewCart(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "NoNameCart";
            }
            cartNames.Add(fileName);
            productList.Clear();
            var response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddCart/{fileName}", fileName);
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddProductsToCart/{fileName}", productList);
        }
        private string _currentcart { get; set; }
        public string CurrentCart
        {
            get { return _currentcart; }
            set { _currentcart = value; }
        }

        public List<string> cartNames = new List<string>();
        public void AddCartNames()
        {
            //string directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
            //var jsonFiles = Directory.EnumerateFiles(directory, "*.json");
            //// Loop through all json files in local appdata folder
            //foreach (string currentFile in jsonFiles)
            //{
            //    // If json file is not Inventory file add it to cartNames list
            //    if (!(currentFile == (directory + @"\Inventory.json")))
            //    {
            //        // Take away the directory path and the .json part of the currentFile string to get only the cart
            //        var cartNameScope = currentFile.Replace(directory, "");
            //        cartNameScope = cartNameScope.Replace(".json", "");
            //        cartNameScope = cartNameScope.Replace("\\", "");
            //        cartNames.Add(cartNameScope);
            //    }
            //}
            var listOfCarts = new WebRequestHandler().Get("http://localhost:5127/Cart").Result;
            cartNames = JsonConvert.DeserializeObject<List<string>>(listOfCarts);
        }
    }
}
