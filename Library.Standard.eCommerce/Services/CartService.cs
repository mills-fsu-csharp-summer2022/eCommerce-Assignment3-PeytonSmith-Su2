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
        // Updates a product in all carts, used when updating a product in inventory
        // it will update all products in all carts
        public void UpdateProductInAllCarts(Product product)
        {
            AddCartNames();
            foreach(var carts in cartNames)
            {
                var productsJson = new WebRequestHandler().Get($"http://localhost:5127/Cart/{carts}").Result;
                var productListTemp = JsonConvert.DeserializeObject<List<Product>>(productsJson);

                var productFound = productListTemp.FirstOrDefault(i => i.UID == product.UID);
                if(productFound != null)
                {
                    // Update from database by deleting/adding product to all carts in database
                    var response = new WebRequestHandler().Post($"http://localhost:5127/Cart/UpdateMetaData/{carts}", product).Result;
                }
            }
        }
        // Deletes a product from all carts, used when deleting a product from inventory
        // it will delete all products in all carts
        public void DeleteProductInAllCarts(Product product)
        {
            AddCartNames();
            foreach (var carts in cartNames)
            {
                var productsJson = new WebRequestHandler().Get($"http://localhost:5127/Cart/{carts}").Result;
                var productListTemp = JsonConvert.DeserializeObject<List<Product>>(productsJson);

                var productFound = productListTemp.FirstOrDefault(i => i.UID == product.UID);
                if (productFound != null)
                {
                    // Delete from database by deleting all products to all carts in database
                    var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/Delete/{carts}/{product.UID}").Result;
                }
            }
        }
        // Check if a product is already in the list, if so set ExistingProductInList
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
            productList = new List<Product>();
        }
        // Add or update product from productList and updates on the server
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
        // Delete product from productList and updates on the server
        public void Delete(int uid)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/Delete/{CurrentCart}/{uid}").Result;
            var productToDelete = productList.FirstOrDefault(t => t.UID == uid);
            if (productToDelete == null)
            {
                return;
            }
            productList.Remove(productToDelete);
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddProductsToCart/{CurrentCart}", productList).Result;
        }
        // Load list of products from a cart from the server
        public void Load(string fileName = null)
        {
            var productsJson = new WebRequestHandler().Get($"http://localhost:5127/Cart/{fileName}").Result;
            productList = JsonConvert.DeserializeObject<List<Product>>(productsJson);
            CurrentCart = fileName;
        }
        // Deletes a cart from the database
        // Used at the end of the program after checkout
        public void DeleteCart(string fileName = null)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/DeleteCart/{CurrentCart}").Result;
        }
        // Saves cart products to the database
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
            var response = new WebRequestHandler().Get($"http://localhost:5127/Cart/DeleteCart/{CurrentCart}").Result;
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddCart/{fileName}", fileName).Result;
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddProductsToCart/{fileName}", productList).Result;
            _currentcart = fileName;
        }

        // Used when adding a new cart on the client, clears the list and updates the info the server
        public void NewCart(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "NoNameCart";
            }
            cartNames.Add(fileName);
            productList.Clear();
            var response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddCart/{fileName}", fileName).Result;
            response = new WebRequestHandler().Post($"http://localhost:5127/Cart/AddProductsToCart/{fileName}", productList).Result;
        }
        private string _currentcart { get; set; }
        public string CurrentCart
        {
            get { return _currentcart; }
            set { _currentcart = value; }
        }
        // Grabs cart names from server and puts it in cartNames
        public List<string> cartNames = new List<string>();
        public void AddCartNames()
        {
            var listOfCarts = new WebRequestHandler().Get("http://localhost:5127/Cart").Result;
            cartNames = JsonConvert.DeserializeObject<List<string>>(listOfCarts);
        }
    }
}
