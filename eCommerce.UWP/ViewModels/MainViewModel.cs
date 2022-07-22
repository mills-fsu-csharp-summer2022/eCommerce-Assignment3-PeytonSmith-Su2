using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using eCommerce.UWP.Dialogs;
using Windows.UI.Xaml.Controls;
using System.IO;
using Windows.UI.Xaml;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;

namespace eCommerce.UWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public string QueryInventory { get; set; }
        public string QueryCart { get; set; }
        public bool NoSortingInventory { get; set; } = true;
        public bool SortNameInventory { get; set; } = false;
        public bool SortUnitPriceInventory { get; set; } = false;
        public bool NoSortingCart { get; set; } = true;
        public bool SortNameCart { get; set; } = false;
        public bool SortTotalPriceCart { get; set; } = false;
        public Product SelectedProductInventory { get; set; }
        public Product SelectedProductCart { get; set; }
        public InventoryService _productServiceInventory;
        public CartService _productServiceCart;

        public ObservableCollection<Product> ProductsInventory
        {
            // Display the list of products in inventory based on query and which sort is toggled
            get
            {
                if (_productServiceInventory == null)
                {
                    return new ObservableCollection<Product>();
                }

                if (string.IsNullOrEmpty(QueryInventory))
                {
                    if (NoSortingInventory)
                    {
                        return new ObservableCollection<Product>(_productServiceInventory.Products);
                    } else if (SortNameInventory)
                    {
                        return new ObservableCollection<Product>(_productServiceInventory.Products.OrderBy(t => t.Name));
                    }
                    else if (SortUnitPriceInventory)
                    {
                        return new ObservableCollection<Product>(_productServiceInventory.Products.OrderBy(t => t.Price));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    if (NoSortingInventory)
                    {
                        return new ObservableCollection<Product>(_productServiceInventory.Products.Where(i => i.Name.ToUpper().Contains(QueryInventory.ToUpper()) || i.Description.ToUpper().Contains(QueryInventory.ToUpper())));
                    } else if (SortNameInventory)
                    {
                        return new ObservableCollection<Product>(_productServiceInventory.Products.Where(i => i.Name.ToUpper().Contains(QueryInventory.ToUpper()) || i.Description.ToUpper().Contains(QueryInventory.ToUpper())).OrderBy(t => t.Name));
                    } else if (SortUnitPriceInventory)
                    {
                        return new ObservableCollection<Product>(_productServiceInventory.Products.Where(i => i.Name.ToUpper().Contains(QueryInventory.ToUpper()) || i.Description.ToUpper().Contains(QueryInventory.ToUpper())).OrderBy(t => t.Price));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public ObservableCollection<Product> ProductsCart
        {
            // Display the list of products in cart based on query and which sort is toggled
            get
            {
                if (_productServiceCart == null)
                {
                    return new ObservableCollection<Product>();
                }

                if (string.IsNullOrEmpty(QueryCart))
                {
                    if (NoSortingCart)
                    {
                        return new ObservableCollection<Product>(_productServiceCart.Products);
                    }
                    else if (SortNameCart)
                    {
                        return new ObservableCollection<Product>(_productServiceCart.Products.OrderBy(t => t.Name));
                    }
                    else if (SortTotalPriceCart)
                    {
                        return new ObservableCollection<Product>(_productServiceCart.Products.OrderBy(t => t.TotalPrice));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    if (NoSortingCart)
                    {
                        return new ObservableCollection<Product>(_productServiceCart.Products.Where(i => i.Name.ToUpper().Contains(QueryCart.ToUpper()) || i.Description.ToUpper().Contains(QueryCart.ToUpper())));
                    }
                    else if (SortNameCart)
                    {
                        return new ObservableCollection<Product>(_productServiceCart.Products.Where(i => i.Name.ToUpper().Contains(QueryCart.ToUpper()) || i.Description.ToUpper().Contains(QueryCart.ToUpper())).OrderBy(t => t.Name));
                    }
                    else if (SortTotalPriceCart)
                    {
                        return new ObservableCollection<Product>(_productServiceCart.Products.Where(i => i.Name.ToUpper().Contains(QueryCart.ToUpper()) || i.Description.ToUpper().Contains(QueryCart.ToUpper())).OrderBy(t => t.TotalPrice));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public MainViewModel()
        {
            _productServiceCart = CartService.Current;
            _productServiceInventory = InventoryService.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task AddInventory(ProductType iType)
        {
            // Check whether the product in quantity/weight and display the respective dialog
            ContentDialog diag = null;
            if (iType == ProductType.Quantity)
            {
                diag = new QuantityDialog();
            }
            else if (iType == ProductType.Weight)
            {
                diag = new WeightDialog();
            }
            else
            {
                throw new NotImplementedException();
            }

            await diag.ShowAsync();
            RefreshInventory();
        }
        public async Task AddProduct()
        {
            // Check whether the product in quantity/weight and display the respective dialog
            ContentDialog diag = null;
            diag = new AddProductToInventory();
            await diag.ShowAsync();
            RefreshInventory();
        }
        public async Task AddCart()
        {
            // Check if the selected product in inventory is by quantity/weight and show the respective dialog
            if (SelectedProductInventory != null)
            {
                ContentDialog diag = null;
                if (SelectedProductInventory is ProductByQuantity)
                {
                    diag = new AddQuantityProductToCart(SelectedProductInventory);
                }
                else if (SelectedProductInventory is ProductByWeight)
                {
                    diag = new AddWeightProductToCart(SelectedProductInventory);
                }
                else
                {
                    throw new NotImplementedException();
                }

                await diag.ShowAsync();
                RefreshCart();
                RefreshInventory();
            }
        }
        public void RemoveInventory()
        {
            var id = SelectedProductInventory?.Id ?? -1;
            if (id >= 0)
            {
                if (_productServiceCart.CheckProductInList(SelectedProductInventory))
                {
                    _productServiceCart.Delete(SelectedProductInventory.UID);
                }
                _productServiceInventory.Delete(SelectedProductInventory.UID);
            }
            NotifyPropertyChanged("ProductsInventory");
        }
        public void RemoveCart()
        {
            var id = SelectedProductCart?.Id ?? -1;

            if (id >= 0)
            {
                // Check if the select product in cart is in inventory, if so update the weight/quantity
                if (_productServiceInventory.CheckProductInList(SelectedProductCart))
                {
                    Product ExistingProduct = _productServiceInventory.ReturnExistingProductInList();
                    if (ExistingProduct is ProductByQuantity)
                    {
                        (ExistingProduct as ProductByQuantity).Quantity += (SelectedProductCart as ProductByQuantity).Quantity;
                    }
                    else if (ExistingProduct is ProductByWeight)
                    {
                        (ExistingProduct as ProductByWeight).Weight += (SelectedProductCart as ProductByWeight).Weight;
                    }
                    _productServiceInventory.AddOrUpdate(ExistingProduct);
                }

                _productServiceCart.Delete(SelectedProductCart.UID);
            }
            NotifyPropertyChanged("ProductsCart");
        }
        public async void UpdateInventory()
        {
            if (SelectedProductInventory != null)
            {
                // Check if selected product in inventory is by quantity/weight and show the respective dialog
                if (SelectedProductInventory is ProductByQuantity)
                {
                    var diag = new QuantityDialog(SelectedProductInventory);
                    await diag.ShowAsync();
                    NotifyPropertyChanged("ProductsInventory");
                }
                else if (SelectedProductInventory is ProductByWeight)
                {
                    var diag = new WeightDialog(SelectedProductInventory);
                    await diag.ShowAsync();
                    NotifyPropertyChanged("ProductsInventory");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        public async void UpdateCart()
        {
            if (SelectedProductCart != null)
            {
                // Check if selected product in cart is by quantity/weight and show the respective dialog
                if (SelectedProductCart is ProductByQuantity)
                {
                    var diag = new UpdateQuantityProductToCart(SelectedProductCart);
                    await diag.ShowAsync();
                    NotifyPropertyChanged("ProductsCart");
                }
                else if (SelectedProductCart is ProductByWeight)
                {
                    var diag = new UpdateWeightProductToCart(SelectedProductCart);
                    await diag.ShowAsync();
                    NotifyPropertyChanged("ProductsCart");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        public async void Save()
        {
            var diag = new Save();
            await diag.ShowAsync();
            NotifyPropertyChanged("ProductsCart");
            NotifyPropertyChanged("ProductsInventory");
        }
        public async void New()
        {
            var diag = new New();
            await diag.ShowAsync();
            NotifyPropertyChanged("ProductsCart");
            NotifyPropertyChanged("ProductsInventory");
        }
        public async void Load()
        {
            NotifyPropertyChanged("CartNames");
            var diag = new Load();
            await diag.ShowAsync();
            NotifyPropertyChanged("ProductsCart");
            NotifyPropertyChanged("ProductsInventory");
        }
        public void SaveCart(string fileName)
        {
            _productServiceCart.Save(fileName);
        }
        public void LoadCart(string fileName)
        {
            _productServiceCart.Load(fileName);
            NotifyPropertyChanged("ProductsCart");
        }

        public void RefreshInventory()
        {
            NotifyPropertyChanged("ProductsInventory");
        }

        public void RefreshCart()
        {
            _productServiceCart.Save(_productServiceCart.CurrentCart);
            NotifyPropertyChanged("ProductsCart");
        }

        // Changes all of the toggle variables depending on which is called
        public void NoSortToggleInventory()
        {
            NoSortingInventory = true;
            SortNameInventory = false;
            SortUnitPriceInventory = false;
        }
        public void NoSortToggleCart()
        {
            NoSortingCart = true;
            SortNameCart = false;
            SortTotalPriceCart = false;
        }
        public void SortNameToggleInventory()
        {
            NoSortingInventory = false;
            SortNameInventory = true;
            SortUnitPriceInventory = false;
        }
        public void SortNameToggleCart()
        {
            NoSortingCart = false;
            SortNameCart = true;
            SortTotalPriceCart = false;
        }
        public void SortUnitPriceToggleInventory()
        {
            NoSortingInventory = false;
            SortNameInventory = false;
            SortUnitPriceInventory = true;
        }
        public void SortTotalPriceToggleCart()
        {
            NoSortingCart = false;
            SortNameCart = false;
            SortTotalPriceCart = true;
        }
        private string selectedCartName;
        public List<string> CartNames
        {
            get { return _productServiceCart.cartNames; }
            set { _productServiceCart.cartNames = value; NotifyPropertyChanged("CartNames"); }
        }
        public void AddCartNames()
        {
            _productServiceCart.AddCartNames();
        }
        public string SelectedCartName
        {
            get { return selectedCartName; }
            set { selectedCartName = value; }
        }
        public void LoadCartInventoryFromSelection()
        {
            if(SelectedCartName != null)
            {
                _productServiceCart.Load(SelectedCartName);
                //_productServiceInventory.Load("Inventory");
            }
        }
        private double _subtotal { get; set; } = 0;
        public double SubTotal
        {
            get { return _subtotal; }
        }
        private double _bogosale { get; set; } = 0;
        public double BogoSale
        {
            get { return _bogosale; }
        }
        public double TaxedAmount { get { return (_subtotal - _bogosale) * 0.07; } }
        public double Total { get { return (_subtotal + TaxedAmount - _bogosale); } }

        public void CalculatePriceCart()
        {
            // Math that calculates the total bogosale and subtotal in cart 
            foreach(var product in _productServiceCart.Products)
            {
                if(product is ProductByQuantity)
                {
                    var poi = product as ProductByQuantity;
                    _subtotal += poi.TotalPrice;
                    if (poi.Bogo)
                    {
                        var couples = poi.Quantity / 2;
                        _bogosale += poi.Price * couples;
                    }
                }
                else if(product is ProductByWeight)
                {
                    var poi = product as ProductByWeight;
                    _subtotal += poi.TotalPrice;
                    if (poi.Bogo)
                    {
                        _bogosale += (poi.Weight * poi.Price) / 2;
                    }
                }
            }
            NotifyPropertyChanged("SubTotal");
            NotifyPropertyChanged("BogoSale");
            NotifyPropertyChanged("TaxedAmount");
            NotifyPropertyChanged("Total");
        }
        public async void PaymentInfo()
        {
            var diag = new PaymentInfo();
            await diag.ShowAsync();
            NotifyPropertyChanged("ProductsCart");
            NotifyPropertyChanged("ProductsInventory");
        }
        public void DeleteCart()
        {
            _productServiceCart.DeleteCart(_productServiceCart.CurrentCart);
        }
    }
}

    public enum ProductType
    {
        Quantity, Weight
    }