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
    public sealed partial class AddProductToInventory : ContentDialog
    {
        public AddProductToInventory()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((DataContext as ProductViewModel).IsQuantity)
            {
                if ((DataContext as ProductViewModel).Name == null)
                {
                    (DataContext as ProductViewModel).Name = " ";
                }
                if ((DataContext as ProductViewModel).Description == null)
                {
                    (DataContext as ProductViewModel).Description = " ";
                }
                ProductService.Current.AddOrUpdate((DataContext as ProductViewModel).BoundProduct);
                ProductService.Current.SetUID((DataContext as ProductViewModel).BoundProduct);
                // Update product in cart with meta data changes
                if (ProductService.Current2.CheckProductInList((DataContext as ProductViewModel).BoundProduct))
                {
                    ProductService.Current2.ReturnExistingProductInList().Name = ((DataContext as ProductViewModel).BoundProduct).Name;
                    ProductService.Current2.ReturnExistingProductInList().Description = ((DataContext as ProductViewModel).BoundProduct).Description;
                    ProductService.Current2.ReturnExistingProductInList().Price = ((DataContext as ProductViewModel).BoundProduct).Price;
                    ProductService.Current2.ReturnExistingProductInList().Bogo = ((DataContext as ProductViewModel).BoundProduct).Bogo;
                }
            }
            else if((DataContext as ProductViewModel).IsWeight)
            {
                if ((DataContext as ProductViewModel).Name == null)
                {
                    (DataContext as ProductViewModel).Name = " ";
                }
                if ((DataContext as ProductViewModel).Description == null)
                {
                    (DataContext as ProductViewModel).Description = " ";
                }
                ProductService.Current.AddOrUpdate((DataContext as ProductViewModel).BoundProduct);
                ProductService.Current.SetUID((DataContext as ProductViewModel).BoundProduct);
                // Update product in cart with meta data changes
                if (ProductService.Current2.CheckProductInList((DataContext as ProductViewModel).BoundProduct))
                {
                    ProductService.Current2.ReturnExistingProductInList().Name = ((DataContext as ProductViewModel).BoundProduct).Name;
                    ProductService.Current2.ReturnExistingProductInList().Description = ((DataContext as ProductViewModel).BoundProduct).Description;
                    ProductService.Current2.ReturnExistingProductInList().Price = ((DataContext as ProductViewModel).BoundProduct).Price;
                    ProductService.Current2.ReturnExistingProductInList().Bogo = ((DataContext as ProductViewModel).BoundProduct).Bogo;
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
