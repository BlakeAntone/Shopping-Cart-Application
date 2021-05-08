using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Library.Product;
using Newtonsoft.Json;

     
namespace Assignment4Application.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public Product SelectedInventoryProduct { get; set; }
        public Product SelectedCartProduct { get; set; }
        public ObservableCollection<Product> Inventory { get; set; }
        public ObservableCollection<Product> Cart { get; set; }

        public MainViewModel()
        {
            Inventory = new ObservableCollection<Product>();
            Cart = new ObservableCollection<Product>();
            NotifyPropertyChanged("Total");
        }

        // Calls NotifyPropertyChanged for total when the application is opened and cart contents have been retrieved from the API
        public void UpdateTotal()
        {
            NotifyPropertyChanged("Total");
        }

        public string Total => $"Subtotal: {Cart.Sum(i => i.Price):C}\n" +
        $"Tax: {Cart.Sum(i => i.Price) * 0.07:C}\t\t" +
        $"Total: {Cart.Sum(i => i.Price) * 1.07:C}";

        /// Adds 1 unit/ounce of the selected Item to the cart 
        public async void AddToCart()
        {
            if (SelectedInventoryProduct == null)
            {
                return;
            }
            var thisProduct = SelectedInventoryProduct;
            if (thisProduct is ProductByQuantity)
            {
                if (Cart.Any(i => i.Id.Equals(thisProduct.Id)))
                {
                    thisProduct = Cart.FirstOrDefault(i => i.Id.Equals(SelectedInventoryProduct.Id)) as ProductByQuantity;
                    (thisProduct as ProductByQuantity).Units++;
                }
                else
                {
                    thisProduct = new ProductByQuantity(SelectedInventoryProduct.Name, SelectedInventoryProduct.Description, (SelectedInventoryProduct as ProductByQuantity).UnitPrice, 1) { Id = SelectedInventoryProduct.Id };
                }  
            }
            else
            {
                if (Cart.Any(i => i.Id.Equals(thisProduct.Id)))
                {
                    thisProduct = Cart.FirstOrDefault(i => i.Id.Equals(SelectedInventoryProduct.Id)) as ProductByWeight;
                    (thisProduct as ProductByWeight).Ounces++;
                }
                else
                {
                    thisProduct = new ProductByWeight(SelectedInventoryProduct.Name, SelectedInventoryProduct.Description, (SelectedInventoryProduct as ProductByWeight).PricePerOunce, 1) { Id = SelectedInventoryProduct.Id };
                }
            }
            thisProduct = JsonConvert.DeserializeObject<Product>(await new WebRequestHandler().Post("http://localhost/Assignment4API/ShoppingCart/AddOrUpdate", thisProduct));

            var index = Cart.IndexOf(Cart.FirstOrDefault(t => t.Id.Equals(thisProduct.Id)));
            if(index < 0)
            {
                Cart.Add(thisProduct);
                NotifyPropertyChanged("Cart");
            }
            else
            {
                Cart.RemoveAt(index);
                Cart.Insert(index, thisProduct);
                NotifyPropertyChanged("Cart");
            }

            SelectedInventoryProduct = null;
            NotifyPropertyChanged("SelectedInventoryProduct");
            NotifyPropertyChanged("Total");
        }

        /// Removes 1 unit/ounce of the slected itewm from the cart and removes the item if Units/ounces hits zero
        public async void RemoveFromCart()
        {
            if (SelectedCartProduct == null)
            {
                return;
            }
            var thisProduct = SelectedCartProduct;
            if (thisProduct is ProductByQuantity)
            {
                (thisProduct as ProductByQuantity).Units--;
                var index = Cart.IndexOf(Cart.FirstOrDefault(t => t.Id.Equals(thisProduct.Id)));
                if ((thisProduct as ProductByQuantity).Units < 1)
                {
                    thisProduct = JsonConvert.DeserializeObject<Product>(await new WebRequestHandler().Post("http://localhost/Assignment4API/ShoppingCart/Delete", thisProduct.Id));
                    Cart.RemoveAt(index);
                }
                else
                {
                    thisProduct = JsonConvert.DeserializeObject<Product>(await new WebRequestHandler().Post("http://localhost/Assignment4API/ShoppingCart/AddOrUpdate", thisProduct));
                    Cart.RemoveAt(index);
                    Cart.Insert(index, thisProduct);
                    NotifyPropertyChanged("Cart");
                }
            }
            else
            {
                (thisProduct as ProductByWeight).Ounces--;
                var index = Cart.IndexOf(Cart.FirstOrDefault(t => t.Id.Equals(thisProduct.Id)));
                if ((thisProduct as ProductByWeight).Ounces < 1)
                {
                    thisProduct = JsonConvert.DeserializeObject<Product>(await new WebRequestHandler().Post("http://localhost/Assignment4API/ShoppingCart/Delete", thisProduct.Id));
                    Cart.RemoveAt(index);
                }
                else
                {
                    thisProduct = JsonConvert.DeserializeObject<Product>(await new WebRequestHandler().Post("http://localhost/Assignment4API/ShoppingCart/AddOrUpdate", thisProduct));
                    Cart.RemoveAt(index);
                    Cart.Insert(index, thisProduct);
                    NotifyPropertyChanged("Cart");
                }
            }

            SelectedCartProduct = null;
            NotifyPropertyChanged("SelectedCartProduct");
            NotifyPropertyChanged("Total");
        }

        public async void ClearCart()
        {
            await new WebRequestHandler().Get("http://localhost/Assignment4API/ShoppingCart/Clear");
            Cart.Clear();
            NotifyPropertyChanged("Cart");
            NotifyPropertyChanged("Total");
        }

        public async void SearchInventory(String queryText)
        {
            var searchList = JsonConvert.DeserializeObject<List<Product>>(await new WebRequestHandler().Post("http://localhost/Assignment4API/Inventory/Search", queryText));
            Inventory.Clear();
            searchList.ForEach(Inventory.Add);

        }

        public void ClearSearch()
        {
            var fullInventory = JsonConvert.DeserializeObject<List<Product>>(( new WebRequestHandler().Get("http://localhost/Assignment4API/Inventory/GetAll").Result));
            Inventory.Clear();
            fullInventory.ForEach(Inventory.Add);
        }

        /// Property for displaying the final receipt and writing it to disk on checkout button click
        public string DisplayReceipt
        {
            get
            {
               var receipt = new WebRequestHandler().Get("http://localhost/Assignment4API/ShoppingCart/GetReceipt").Result;
                return receipt;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
