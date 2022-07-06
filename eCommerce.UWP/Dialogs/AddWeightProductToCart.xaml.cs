using Library.eCommerce.Models;
using Library.eCommerce.Services;
using eCommerce.UWP.ViewModels;
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
    public sealed partial class AddWeightProductToCart : ContentDialog
    {
        private ProductByWeight product;
        public AddWeightProductToCart()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByWeight();
        }

        public AddWeightProductToCart(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = new ProductByWeight();
            product = selectedItem as ProductByWeight;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Copy data from selected item to product in cart and update quantity/weight in inventory
            var DataContextProduct = DataContext as ProductByWeight;

            DataContextProduct.Name = product.Name;
            DataContextProduct.Description = product.Description;
            DataContextProduct.Price = product.Price;
            DataContextProduct.Bogo = product.Bogo;
            DataContextProduct.UID = product.UID;
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
            // Check if the product is in the cart already, if so, add the weight to the existing item
            if (ProductService.Current2.CheckProductInList(product))
            {
                Product ExistingProduct = ProductService.Current2.ReturnExistingProductInList();
                (ExistingProduct as ProductByWeight).Weight += DataContextProduct.Weight;
            }
            else
            {
                ProductService.Current2.AddOrUpdate(DataContextProduct);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
