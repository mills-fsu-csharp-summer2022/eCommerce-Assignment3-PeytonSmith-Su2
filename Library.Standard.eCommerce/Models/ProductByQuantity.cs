using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;

namespace Library.eCommerce.Models
{
    [JsonConverter(typeof(ProductJsonConverter))]
    public class ProductByQuantity : Product
    {
        new public int Quantity { get; set; }
        new public int QuantityTest { get; set; }

        public override double TotalPrice { get { return Price * Quantity; } set { } }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo} - Quantity: {Quantity}";
        }
    }
}
