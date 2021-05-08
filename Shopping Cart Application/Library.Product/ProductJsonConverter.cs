using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Product
{
    public class ProductJsonConverter : JsonCreationConverter<Product>
    {
        protected override Product Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["UnitPrice"] != null || jObject["unitPrice"] != null)
            {
                return new ProductByQuantity();
            }
            else if (jObject["PricePerOunce"] != null || jObject["pricePerOunce"] != null)
            {
                return new ProductByWeight();
            }
            else
            {
                return null;
            }

        }
    }
}
