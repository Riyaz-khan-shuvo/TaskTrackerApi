using Microsoft.AspNetCore.Http;

namespace TaskTracker.Shared.Exceptions
{
    public class ForbiddenSubsciptionException : Exception
    {
        public ForbiddenSubsciptionException() : base(StatusCodes.Status402PaymentRequired.ToString())
        {

        }
    }
}
