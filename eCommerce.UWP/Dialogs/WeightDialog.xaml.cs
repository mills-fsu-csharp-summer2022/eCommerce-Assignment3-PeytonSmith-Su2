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
    public sealed partial class WeightDialog : ContentDialog
    {
        private double previousWeight;
        public WeightDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByWeight();

        }

        public WeightDialog(Product selectedItem)
        {
            this.InitializeComponent();
            this.DataContext = selectedItem;
            previousWeight = (selectedItem as ProductByWeight).Weight;
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((DataContext as ProductByWeight).Weight < 0)
            {
                (DataContext as ProductByWeight).Weight = previousWeight;
                return;
            }
            if ((DataContext as ProductByWeight).Name == null)
            {
                (DataContext as ProductByWeight).Name = " ";
            }
            if ((DataContext as ProductByWeight).Description == null)
            {
                (DataContext as ProductByWeight).Description = " ";
            }
            InventoryService.Current.AddOrUpdate(DataContext as ProductByWeight);
            // Update product in cart with meta data changes
            if (CartService.Current.CheckProductInList(DataContext as ProductByQuantity))
            {
                CartService.Current.ReturnExistingProductInList().Name = (DataContext as ProductByWeight).Name;
                CartService.Current.ReturnExistingProductInList().Description = (DataContext as ProductByWeight).Description;
                CartService.Current.ReturnExistingProductInList().Price = (DataContext as ProductByWeight).Price;
                CartService.Current.ReturnExistingProductInList().Bogo = (DataContext as ProductByWeight).Bogo;
                CartService.Current.AddOrUpdate(CartService.Current.ReturnExistingProductInList());
                CartService.Current.UpdateProductInAllCarts(CartService.Current.ReturnExistingProductInList());
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
