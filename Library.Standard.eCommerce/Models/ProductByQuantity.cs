using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public class ProductByQuantity : Product
    {
        public int Quantity { get; set; }

        public override double TotalPrice { get { return Price * Quantity; } set { } }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo} - Quantity: {Quantity}";
        }
    }
}
