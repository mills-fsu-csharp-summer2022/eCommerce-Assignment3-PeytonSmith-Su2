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
    public sealed partial class AddQuantityProductToCart : ContentDialog
    {
        private ProductByQuantity product;
        public AddQuantityProductToCart()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByQuantity();
        }

        public AddQuantityProductToCart(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = new ProductByQuantity();
            product = selectedItem as ProductByQuantity;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Copy data from selected item to product in cart and update quantity/weight in inventory
            var DataContextProduct = DataContext as ProductByQuantity;

            DataContextProduct.Name = product.Name;
            DataContextProduct.Description = product.Description;
            DataContextProduct.Price = product.Price;
            DataContextProduct.Bogo = product.Bogo;
            DataContextProduct.UID = product.UID;
            if(DataContextProduct.Quantity <= 0)
            {
                return;
            }
            // If requested quantity is greater than quantity in inventory take all inventory quantity
            if (DataContextProduct.Quantity > product.Quantity)
            {
                DataContextProduct.Quantity = product.Quantity;
                product.Quantity = 0;
            }
            else
            {
                product.Quantity -= DataContextProduct.Quantity;
            }
            // Check if the product is in the cart already, if so, add the quantity to the existing item
            if (ProductService.Current2.CheckProductInList(product))
            {
               Product ExistingProduct = ProductService.Current2.ReturnExistingProductInList();
                (ExistingProduct as ProductByQuantity).Quantity += DataContextProduct.Quantity; 
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
