using DeliveryManager.Models;
using System.Net;

namespace DeliveryManager.Infrastructure.Exception
{
    public class ValidationException : System.Exception
    {
        public ErrorCodes ErrorCodes { get; }
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

        public ValidationException(string message, ErrorCodes errorCodes = ErrorCodes.BadArgument) : base(message)
        {
            ErrorCodes = errorCodes;
        }

        public ValidationException(string message, ErrorCodes errorCodes, HttpStatusCode statusCode)
            : this(message, errorCodes)
        {
            StatusCode = statusCode;
        }
    }
}
