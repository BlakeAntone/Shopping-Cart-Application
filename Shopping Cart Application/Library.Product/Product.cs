using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.Product
{
    [JsonConverter(typeof(ProductJsonConverter))]
    public abstract class Product : INotifyPropertyChanged
    {
        public virtual double Price { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }

        public string Display
        {
            get
            {
                return $"{Name} - {Description}";
            }
        }

        /// Event Handler for Updating any chnaged properties in the view
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// Base Product constructor
        public Product(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            this.Id = System.Guid.NewGuid();
        }

        public Product()
        {
        }

        /// Abstract method for displaying an item line within the receipt
        public abstract string DisplayReceiptLine();

    }

    /// <summary>
    /// Products priced per each unit of product purchased
    /// </summary>
    public class ProductByQuantity : Product
    {
        private int unitsField;
        public double UnitPrice { get; set; }
        public int Units
        {
            get
            {
                return unitsField;
            }
            set
            {
                unitsField = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("DisplayCart");
            }
        }
        public override double Price
        {
            get => UnitPrice * Units;
        }

        public ProductByQuantity(string Name, string Description, double UnitPrice, int Units) : base(Name, Description)
        {
            this.UnitPrice = UnitPrice;
            this.Units = Units;
        }

        public ProductByQuantity()
        {
        }

        public string DisplayCart
        {
            get
            {
                return base.Display + $"\n{Units} at ${UnitPrice} per item = ${Price}";
            }
        }

        public string DisplayInventory
        {
            get
            {
                return base.Display + $"\n${UnitPrice} per item";
            }
        }

        public override string DisplayReceiptLine()
        {
            return this.Name + "\n" + this.Units + " @\t" + this.UnitPrice.ToString("F") + "\t\t $" + this.Price.ToString("F") + "\n";
        }
    }

    /// <summary>
    /// Product priced by each ounce of product purchased
    /// </summary>
    public class ProductByWeight : Product
    {
        private double ouncesField;
        public double PricePerOunce { get; set; }
        public double Ounces
        {
            get
            {
                return ouncesField;
            }
            set
            {
                ouncesField = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("DisplayCart");
            }
        }
        public override double Price { get => PricePerOunce * Ounces; }
        public ProductByWeight(string Name, string Description, double PricePerOunce, double Ounces) : base(Name, Description)
        {
            this.PricePerOunce = PricePerOunce;
            this.Ounces = Ounces;
        }

        public ProductByWeight()
        {
        }
        public string DisplayCart
        {
            get
            {
                return base.Display + $"\n{Ounces} oz at ${PricePerOunce} per ounce = ${Price}";
            }
        }

        public string DisplayInventory
        {
            get
            {
                return base.Display + $"\n${PricePerOunce} per Ounce";
            }
        }

        public override string DisplayReceiptLine()
        {
            return this.Name + "\n" + this.Ounces + "oz @\t" + this.PricePerOunce.ToString("F") + "/ oz\t $" + this.Price.ToString("F") + "\n";
        }
    }
}
