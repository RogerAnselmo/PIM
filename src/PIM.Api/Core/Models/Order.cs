using System.Collections.Generic;
using System.Linq;
using PIM.Api.Models.Base;
using PIM.Api.TransferObjects.Requests;

namespace PIM.Api.Core.Models
{
    public class Order : BaseEntity
    {
        public Order() { }

        public Order(NewOrderRequest newOrderRequest)
        {
            BuyerName = newOrderRequest.BuyerName;
            DeliveryAdress = newOrderRequest.DeliveryAdress;
        }
        public string BuyerName { get; set; }
        public string DeliveryAdress { get; set; }
        public ICollection<ProductOrder> Products { get; set; }

        public void SetProductList(IEnumerable<Product> products)
        {
            Products = new List<ProductOrder>();

            if (!products.Any()) return;

            foreach (var p in products)
            {
                var productOrder = new ProductOrder
                {
                    Order = new Order {Id = Id}, Product = p
                };


                Products.Add(productOrder);
            }
        }

        public decimal? Price { get; set; }
    }
}
