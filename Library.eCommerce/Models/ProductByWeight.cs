using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Models
{
    public class ProductByWeight : Product
    {
        public decimal Weight { get; set; }

        public override decimal TotalPrice { get { return Price * Weight; } set { } }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo} - Weight: {Weight}";
        }
    }
}
