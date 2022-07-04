using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace eCommerce.UWP.Dialogs
{
    public sealed partial class UpdateWeightProductToCart : ContentDialog
    {
        private ProductByWeight product;
        private double previousWeight;
        public UpdateWeightProductToCart()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByWeight();
        }

        public UpdateWeightProductToCart(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = selectedItem as ProductByWeight;
            previousWeight = (selectedItem as ProductByWeight).Weight;
            product = selectedItem as ProductByWeight;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var DataContextProduct = DataContext as ProductByWeight;
            if (DataContextProduct.Weight <= 0)
            {
                DataContextProduct.Weight = previousWeight;
                return;
            }

            // Check if selected product for edit is in inventory, if so return that product in inventory and get the weight
            // If the requested weight is greater than the inventory's weight, take all inventory weight.
            if (ProductService.Current.CheckProductInList(product))
            {
                Product ExistingProduct = ProductService.Current.ReturnExistingProductInList();
                if ((DataContext as ProductByWeight).Weight > (ExistingProduct as ProductByWeight).Weight+previousWeight)
                {
                    (DataContext as ProductByWeight).Weight = (ExistingProduct as ProductByWeight).Weight+previousWeight;
                    (ExistingProduct as ProductByWeight).Weight = 0;
                }
                else
                {
                    (ExistingProduct as ProductByWeight).Weight += (previousWeight - DataContextProduct.Weight);
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
