using System;
using Library.Product;
using System.Linq;
using System.Collections.Generic;

namespace Assignment4API
{
    public class DataContext
    {
        public static List<Product> Inventory = new List<Product>
        {
            new ProductByQuantity("Hass Avocado", "Fresh California hass avocado", 0.99, 0),
            new ProductByQuantity("Cucumber", "Fresh Cucumber", 1.25, 0),
            new ProductByQuantity("Zuchinni", "Fresh Zuchinni", 1.15, 0),
            new ProductByQuantity("Bakers Potato", "Fresh Bakers Potato", 0.99, 0),
            new ProductByQuantity("Avocado", "Fresh Florida avocado", 1.05, 0),
            new ProductByQuantity("Sweet Potato", "Fresh Sweet Potato", 1.49, 0),
            new ProductByQuantity("Watermelon", "Fresh whole Watermelon", 3.99, 0),
            new ProductByQuantity("Canteloupe", "Fresh whole Canteloupe", 2.49, 0),
            new ProductByQuantity("Green Bell Pepper", "Fresh Green Bell Pepper", 1.25, 0),
            new ProductByQuantity("Red Bell Pepper", "Fresh Red Bell Pepper", 1.25, 0),
            new ProductByWeight("White Grapes", "Fresh White Grapes", 0.35, 0),
            new ProductByWeight("Red Grapes", "Fresh Red Grapes", 0.35, 0),
            new ProductByWeight("Blueberries", "Fresh Blueberries", 0.45, 0),
            new ProductByWeight("Strawberries", "Fresh Strawberries", 0.55, 0),
            new ProductByWeight("Blackberries", "Fresh Blackberries", 0.45, 0),
            new ProductByWeight("Rasberries", "Fresh Rasberries", 0.45, 0),
            new ProductByWeight("Bananas", "Fresh Bananas", 0.07, 0),
            new ProductByWeight("Gala Apple", "Fresh Gala Apples", 0.15, 0),
            new ProductByWeight("Fuji Apple", "Fresh Fuji Apples", 0.13, 0),
           new ProductByWeight("Peach", "Fresh Georgia Peaches", 0.55, 0)
        };

        public static List<Product> Cart = new List<Product>();
    }
}
