using eCommerce.UWP.ViewModels;
using Library.eCommerce.Models;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace eCommerce.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CartPage : Page
    {
        public CartPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).RefreshCart();
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CheckOutPage));
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage));
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var model = ((sender as Button).DataContext as Product);
            vm.SelectedProductCart = model;
            if (vm != null)
            {
                vm.UpdateCart();
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var model = ((sender as Button).DataContext as Product);
            vm.SelectedProductCart = model;
            if (vm != null)
            {
                vm.RemoveCart();
            }
        }

        private void SaveCartButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.Save();
            }
        }

        private void LoadCartButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.Load();
            }
        }

        private void NoSortButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.NoSortToggleCart();
            vm.RefreshCart();
        }

        private void NameSortButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.SortNameToggleCart();
            vm.RefreshCart();
        }

        private void TotalPriceSortButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.SortTotalPriceToggleCart();
            vm.RefreshCart();
        }
    }
}
