using Library.eCommerce.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.eCommerce.Utilities
{
    public class ProductJsonConverter : JsonCreationConverter<Product>
    {
        protected override Product Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["WeightTest"] != null || jObject["weightTest"] != null)
            {
                return new eCommerce.Models.ProductByWeight();
            }
            else if (jObject["QuantityTest"] != null || jObject["quantityTest"] != null)
            {
                return new eCommerce.Models.ProductByQuantity();
            }
            else
            {
                return new Product();
            }
        }
    }
}
