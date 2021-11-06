using System.Collections.Generic;
using PIM.Api.Models.Base;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ProductPhoto> Photos { get; set; }

        public void UpdateValues(UpdateProduct updateProduct)
        {
            Name = updateProduct.Name;
            Category = updateProduct.Category;
            Brand = updateProduct.Brand;
            Color = updateProduct.Color;
            Price = updateProduct.Price;
            Description = updateProduct.Description;
        }
    }
}
