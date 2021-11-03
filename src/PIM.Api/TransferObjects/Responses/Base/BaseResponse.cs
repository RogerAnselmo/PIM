namespace PIM.Api.TransferObjects.Responses.Base
{
    public class BaseResponse
    {
        public BaseResponse(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
