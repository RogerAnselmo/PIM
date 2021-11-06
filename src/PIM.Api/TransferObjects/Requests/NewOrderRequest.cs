using System.Collections.Generic;

namespace PIM.Api.TransferObjects.Requests
{
    public class NewOrderRequest
    {
        public List<int> ProductIdList { get; set; }
        public string BuyerName { get; set; }
        public string DeliveryAdress { get; set; }
    }
}
