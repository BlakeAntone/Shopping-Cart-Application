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
using Assignment4Application.ViewModels;
using Library.Product;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;

namespace Assignment4Application
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();

            var handler = new WebRequestHandler();
            var products = JsonConvert.DeserializeObject<List<Product>>(handler.Get("http://localhost/Assignment4API/Inventory/GetAll").Result);
            var cartProducts = JsonConvert.DeserializeObject<List<Product>>(handler.Get("http://localhost/Assignment4API/ShoppingCart/GetAll").Result);
            var context = DataContext as MainViewModel;
            products.ForEach(context.Inventory.Add);
            cartProducts.ForEach(context.Cart.Add);
            context.UpdateTotal();
        }

        private void Inventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MainViewModel).AddToCart();
        }

        private void Cart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MainViewModel).RemoveFromCart();
        }

        private void Clear_Cart(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ClearCart();
        }

        private void Search_Inventory(object sender, SearchBoxQuerySubmittedEventArgs e)
        {
            (DataContext as MainViewModel).SearchInventory(e.QueryText);

        }

        private void Clear_Search(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ClearSearch();
        }

        private void Checkout(object sender, RoutedEventArgs e)
        {
           // (DataContext as MainViewModel).Save();
            this.Frame.Navigate(typeof(ReceiptPage), DataContext as MainViewModel);
        }

    }
}
