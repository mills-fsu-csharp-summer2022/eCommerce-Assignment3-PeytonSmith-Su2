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
    public sealed partial class UpdateQuantityProductToCart : ContentDialog
    {
        private ProductByQuantity product;
        private int previousQuantity;
        public UpdateQuantityProductToCart()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByQuantity();
        }

        public UpdateQuantityProductToCart(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = selectedItem as ProductByQuantity;
            previousQuantity = (selectedItem as ProductByQuantity).Quantity;
            product = selectedItem as ProductByQuantity;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var DataContextProduct = DataContext as ProductByQuantity;
            if (DataContextProduct.Quantity <= 0)
            {
                DataContextProduct.Quantity = previousQuantity;
                return;
            }

            // Check if selected product for edit is in inventory, if so return that product in inventory and get the quantity
            // If the requested quantity is greater than the inventory's quantity, take all inventory quantity.
            if (ProductService.Current.CheckProductInList(product))
            {
               Product ExistingProduct = ProductService.Current.ReturnExistingProductInList();
               if((DataContext as ProductByQuantity).Quantity > (ExistingProduct as ProductByQuantity).Quantity+previousQuantity){
                    (DataContext as ProductByQuantity).Quantity = (ExistingProduct as ProductByQuantity).Quantity+previousQuantity;
                    (ExistingProduct as ProductByQuantity).Quantity = 0;
                }
                else
                {
                    (ExistingProduct as ProductByQuantity).Quantity += (previousQuantity - DataContextProduct.Quantity);
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
