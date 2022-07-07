using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace eCommerce.UWP.ViewModels
{
    internal class ProductViewModel : INotifyPropertyChanged
    {
        public string Name { get { return BoundProduct?.Name ?? string.Empty; } set { if (BoundProduct == null) { return; } BoundProduct.Name = value; } }
        public string Description { get { return BoundProduct?.Description ?? string.Empty; } set { if (BoundProduct == null) { return; } BoundProduct.Description = value; } }
        public double Price { get { return BoundProduct?.Price ?? 0; } set { if (BoundProduct == null) { return; } BoundProduct.Price = value; } }
        public int Id { get { return BoundProduct?.Id ?? 0; }}
        public bool Bogo { get { return BoundProduct?.Bogo ?? false; } set { if (BoundProduct == null) { return; } BoundProduct.Bogo = value; } }
        public int Quantity { get { return (BoundProduct as ProductByQuantity)?.Quantity ?? 0; } set { if (BoundProduct == null) { return; } (BoundProduct as ProductByQuantity).Quantity = value; } }
        public double Weight { get { return (BoundProduct as ProductByWeight)?.Weight ?? 0; } set { if (BoundProduct == null) { return; } (BoundProduct as ProductByWeight).Weight = value; } }
        public virtual double TotalPrice { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ProductViewModel()
        {
            boundWeight = null;
            boundQuantity = new ProductByQuantity();
        }
        public bool IsQuantity
        {
            get
            {
                return BoundQuantity != null;
            }

            set
            {
                if (value)
                {
                    boundQuantity = new ProductByQuantity();
                    boundWeight = null;
                    NotifyPropertyChanged("IsQuantityVisible");
                    NotifyPropertyChanged("IsWeightVisible");
                }
            }
        }

        public bool IsWeight
        {
            get
            {
                return BoundWeight != null;
            }

            set
            {
                if (value)
                {
                    boundWeight = new ProductByWeight();
                    boundQuantity = null;
                    NotifyPropertyChanged("IsQuantityVisible");
                    NotifyPropertyChanged("IsWeightVisible");
                }
            }
        }

        private ProductByQuantity boundQuantity;
        public ProductByQuantity BoundQuantity
        {
            get
            {
                return boundQuantity;
            }
        }

        private ProductByWeight boundWeight;
        public ProductByWeight BoundWeight
        {
            get
            {
                return boundWeight;
            }
        }
        public Visibility IsQuantityVisible
        {
            get
            {
                return BoundQuantity == null && BoundWeight != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsWeightVisible
        {
            get
            {
                return BoundWeight == null && BoundQuantity != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Product BoundProduct
        {
            get
            {
                if (BoundQuantity != null)
                {
                    return BoundQuantity;
                }

                return BoundWeight;
            }
        }
        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo}";
        }
    }
}