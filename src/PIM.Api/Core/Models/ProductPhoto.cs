using PIM.Api.Models.Base;

namespace PIM.Api.Core.Models
{
    public class ProductPhoto: BaseEntity
    {
        public string Url { get; set; }
        public string Description { get; set; }
    }
}
