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


namespace Assignment4Application
{
    public sealed partial class ReceiptPage : Page
    {
        public ReceiptPage()
        {
            this.InitializeComponent();
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), DataContext as MainViewModel);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = e.Parameter as MainViewModel;
        }
    }
}
