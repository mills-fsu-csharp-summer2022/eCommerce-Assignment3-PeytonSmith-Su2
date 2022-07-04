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
                return;
            }
            // If requested weight is greater than weight in inventory take all inventory weight
            if (DataContextProduct.Weight > product.Weight)
            {
                DataContextProduct.Weight = product.Weight;
                product.Weight = 0;
            }
            else
            {
                product.Weight -= DataContextProduct.Weight;
            }
            // Check if the product is in the inventory, if so, update the weight of the product in inventory
            if (ProductService.Current.CheckProductInList(product))
            {
                Product ExistingProduct = ProductService.Current.ReturnExistingProductInList();
                (ExistingProduct as ProductByWeight).Weight += (previousWeight - DataContextProduct.Weight);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
