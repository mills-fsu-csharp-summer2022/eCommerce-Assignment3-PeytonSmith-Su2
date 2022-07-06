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
                return productList;
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

                return current;
            }
        }
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

        public static ProductService Current2
        {
            get
            {
                if (current2 == null)
                {
                    current2 = new ProductService();
                }

                return current2;
            }
        }

        public ProductService()
        {
            productList = new List<Product>();
        }

        public void AddOrUpdate(Product product)
        {
            if (product.Id <= 0)
            {
                product.Id = NextId;
                Products.Add(product);
            }

        }

        public void Delete(int uid)
        {
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
