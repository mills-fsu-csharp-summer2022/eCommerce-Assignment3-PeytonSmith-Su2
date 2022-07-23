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
    public class InventoryService
    {
        private string persistPath
    = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private static List<Product> productList;

        public List<Product> Products
        {
            get
            {
                var productsInventoryJson = new WebRequestHandler().Get("http://localhost:5127/Inventory").Result;
                productList = JsonConvert.DeserializeObject<List<Product>>(productsInventoryJson);
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
        public int NextUid
        {
            get
            {
                if (!Products.Any())
                {
                    return 1;
                }

                return Products.Select(t => t.UID).Max() + 1;
            }
        }
        private static InventoryService current;

        public static InventoryService Current
        {
            get
            {
                if (current == null)
                {
                    current = new InventoryService();
                }
                return current;
            }
        }
        // Should only ever be used when adding to inventory
        public void SetUID(Product product)
        {
            product.UID = NextUid;
        }
        // Checks if a specific product is in the list, if so set ExistingProductInList
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

        // Grab all products from the server at startup
        public InventoryService()
        {
            var productsInventoryJson = new WebRequestHandler().Get("http://localhost:5127/Inventory").Result;
            productList = JsonConvert.DeserializeObject<List<Product>>(productsInventoryJson);
        }
        // Add or updates a product in productList and updates on the server
        public void AddOrUpdate(Product product)
        {
            var response = new WebRequestHandler().Post("http://localhost:5127/Inventory/AddOrUpdate", product).Result;
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
            response = new WebRequestHandler().Post($"http://localhost:5127/Inventory/AddProductsToInventory", productList).Result;
        }
        // Deletes a product in productList and updates on the server
        public void Delete(int uid)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5127/Inventory/Delete/{uid}").Result;
            var productToDelete = productList.FirstOrDefault(t => t.Id == uid);
            if (productToDelete == null)
            {
                return;
            }
            productList.Remove(productToDelete);
        }
    }
}
