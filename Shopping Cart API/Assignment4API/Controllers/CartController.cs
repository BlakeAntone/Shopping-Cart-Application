using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Library.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment4API.Controllers
{
    [ApiController]
    [Route("ShoppingCart")]
    public class CartController : Controller
    {
        [HttpGet("GetAll")]
        public ActionResult<List<Product>> GetCart()
        {
            return Ok(DataContext.Cart);
        }

        [HttpGet("Clear")]
        public void Clear()
        {
            DataContext.Cart.Clear();
        }

        [HttpPost("AddOrUpdate")]
        public ActionResult<Product> AddOrUpdate([FromBody]Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }

            var index = DataContext.Cart.IndexOf(DataContext.Cart.FirstOrDefault(t => t.Id.Equals(product.Id)));
            if (index < 0)
            {
                DataContext.Cart.Add(product);
            }
            else
            {
                var productToSync = DataContext.Cart.FirstOrDefault(t => t.Id.Equals(product.Id));
                DataContext.Cart.RemoveAt(index);
                DataContext.Cart.Insert(index, product);
            }
            return Ok(product);
        }

        [HttpPost("Delete")]
        public ActionResult<Product> Delete([FromBody]Guid id)
        {
            var productToRemove = DataContext.Cart.FirstOrDefault(t => t.Id.Equals(id));
            if(productToRemove?.Id != Guid.Empty)
            {
                DataContext.Cart.Remove(productToRemove);
            }
            return productToRemove;
        }

        [HttpGet("GetReceipt")]
        public ActionResult<String> GetReceipt()
        {
            double subtotal = 0.0;
            string receipt = "";
            foreach (Product item in DataContext.Cart)
            {
                subtotal += item.Price;
                string line = item.DisplayReceiptLine();
                receipt += line;
            }
            receipt += ("\n\nSubtotal\t\t" + subtotal.ToString("F"));
            double salesTax = subtotal * 0.07;
            receipt += ("\nSales tax\t\t" + salesTax.ToString("F"));
            receipt += ("\nGrand total\t\t" + (subtotal + salesTax).ToString("F"));
            return receipt;
        }
    }
}
