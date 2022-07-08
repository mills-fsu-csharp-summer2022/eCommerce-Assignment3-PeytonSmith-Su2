using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Library.eCommerce.Models
{
    public partial class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Id { get; set; }
        public bool Bogo { get; set; }
        public virtual double TotalPrice { get; set; }
        // unique ID that is given to a product and is kept throughout cart/inventory
        public int UID { get; set; }
        public int Quantity
        {
            get
            {
                if (IsQuantity)
                {
                    return (this as ProductByQuantity).Quantity;
                }
                return -1;
            }
        }
        public double Weight
        {
            get
            {
                if (IsWeight)
                {
                    return (this as ProductByWeight).Weight;
                }
                return -1;
            }
        }
        public Product()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsQuantity
        {
            get { return this is ProductByQuantity; }
        }
        public bool IsWeight
        {
            get { return this is ProductByWeight; }
        }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo}";
        }
    }
}
