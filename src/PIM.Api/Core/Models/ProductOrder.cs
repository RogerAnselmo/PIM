using PIM.Api.Models.Base;

namespace PIM.Api.Core.Models
{
    public class ProductOrder: BaseEntity
    {
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
