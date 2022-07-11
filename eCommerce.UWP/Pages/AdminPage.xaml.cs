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
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var model = ((sender as Button).DataContext as Product);
            vm.SelectedProductInventory = model;
            if (vm != null)
            {
                vm.UpdateInventory();
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var model = ((sender as Button).DataContext as Product);
            vm.SelectedProductInventory = model;
            if (vm != null)
            {
                vm.RemoveInventory();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).RefreshInventory();
        }

        private void NoSortButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.NoSortToggleInventory();
            vm.RefreshInventory();
        }

        private void NameSortButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.SortNameToggleInventory();
            vm.RefreshInventory();
        }

        private void UnitPriceSortButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            vm.SortUnitPriceToggleInventory();
            vm.RefreshInventory();
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.AddProduct();
            }
        }
    }
}
