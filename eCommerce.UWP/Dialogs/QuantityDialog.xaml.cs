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
    public sealed partial class QuantityDialog : ContentDialog
    {
        private int previousQuantity;
        public QuantityDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByQuantity();

        }

        public QuantityDialog(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = selectedItem;
            previousQuantity = (selectedItem as ProductByQuantity).Quantity;
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if((DataContext as ProductByQuantity).Quantity < 0)
            {
                (DataContext as ProductByQuantity).Quantity = previousQuantity;
                return;
            }
            if ((DataContext as ProductByQuantity).Name == null)
            {
                (DataContext as ProductByQuantity).Name = " ";
            }
            if ((DataContext as ProductByQuantity).Description == null)
            {
                (DataContext as ProductByQuantity).Description = " ";
            }
            ProductService.Current.AddOrUpdate(DataContext as ProductByQuantity);
            ProductService.Current.SetUID(DataContext as ProductByQuantity);
            // Update product in cart with meta data changes
            if (ProductService.Current2.CheckProductInList(DataContext as ProductByQuantity))
            {
                ProductService.Current2.ReturnExistingProductInList().Name = (DataContext as ProductByQuantity).Name;
                ProductService.Current2.ReturnExistingProductInList().Description = (DataContext as ProductByQuantity).Description;
                ProductService.Current2.ReturnExistingProductInList().Price = (DataContext as ProductByQuantity).Price;
                ProductService.Current2.ReturnExistingProductInList().Bogo = (DataContext as ProductByQuantity).Bogo;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
