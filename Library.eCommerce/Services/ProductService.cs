using Library.eCommerce.Models;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class ProductService
    {
        private List<Product> productList;
        private ListNavigator<Product> listNavigator;
        private ListNavigator<Product> sortedNameListNavigator;
        private ListNavigator<Product> sortedTotalListNavigator;
        private ListNavigator<Product> sortedUnitListNavigator;
        private ListNavigator<Product> filteredList;
        public ListNavigator<Product> ListNavigator
        {
            get { return listNavigator; }
        }
        public ListNavigator<Product> SortedNameListNavigator
        {
            get { return sortedNameListNavigator; }
        }
        public ListNavigator<Product> SortedTotalListNavigator
        {
            get { return sortedTotalListNavigator; }  
        }
        public ListNavigator<Product> SortedUnitListNavigator
        {
            get { return sortedUnitListNavigator; }
        }

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
                    return 0;
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
            listNavigator = new ListNavigator<Product>(productList);
            sortedNameListNavigator = new ListNavigator<Product>(productList.OrderBy(t => t.Name));
            sortedTotalListNavigator = new ListNavigator<Product>(productList.OrderBy(t => t.TotalPrice));
            sortedUnitListNavigator = new ListNavigator<Product>(productList.OrderBy(t => t.Price));
        }

        public void Create(Product product)
        {
            product.Id = NextId;
            Products.Add(product);
        }

        public void Update(Product product)
        {

        }

        public void Delete(int id)
        {
            var productToDelete = productList.FirstOrDefault(t => t.Id == id);
            if (productToDelete == null)
            {
                return;
            }
            productList.Remove(productToDelete);
        }

        public void Load(string fileName)
        {
            var productsJson = File.ReadAllText(fileName);
            productList = JsonConvert.DeserializeObject<List<Product>>(productsJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }) ?? new List<Product>();

        }

        public void Save(string fileName)
        {
            var productsJson = JsonConvert.SerializeObject(productList, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
            File.WriteAllText(fileName, productsJson);
        }

        private string _query;

        public ListNavigator<Product> Search(string query)
        {
            filteredList = new ListNavigator<Product>(productList.Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                        || i.Name.Contains(_query))));
            _query = query;
            return filteredList;
        }

    }
}
