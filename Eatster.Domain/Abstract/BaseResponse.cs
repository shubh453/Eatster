namespace Eatster.Domain.Abstract
{
    public abstract class BaseResponse
    {
        public bool Success { get; }
        public string Message { get; }

        protected BaseResponse(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}