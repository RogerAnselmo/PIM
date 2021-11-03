using PIM.Api.Models.Base;

namespace PIM.Api.Models
{
    public class SystemUser: BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
