using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Product;

namespace Assignment4API.Controllers
{
    [ApiController]
    [Route("Inventory")]
    public class InventoryController : ControllerBase
    {
        [HttpGet("GetAll")]
        public ActionResult<List<Product>> GetInventory()
        {
            return Ok(DataContext.Inventory);
        }

        [HttpPost("Search")]
        public ActionResult<List<Product>> SearchInventory([FromBody] String searchTerm)
        {
            List<Product> searchList = new List<Product>();
            foreach(var item in DataContext.Inventory.Where(item => item.Name.Contains(searchTerm) || item.Description.Contains(searchTerm))) {
                searchList.Add(item);
            }
            return searchList;
        }
    }
}
