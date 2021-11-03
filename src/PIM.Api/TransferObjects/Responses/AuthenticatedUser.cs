using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.Api.TransferObjects.Responses
{
    public class AuthenticatedUser: BaseResponse
    {
        public AuthenticatedUser(string message, bool success = true) : base(message, success) { }
        public AuthenticatedUser(string name, string token, string message = "User Successfully athenticated", bool success = true)
            : base(message, success)
        {
            Name = name;
            Token = token;
        }

        public string Name { get; }
        public string Token { get; }
    }
}
