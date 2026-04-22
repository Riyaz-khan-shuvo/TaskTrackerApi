using Microsoft.AspNetCore.Http;

namespace TaskTracker.Shared.Exceptions
{
    public class ForbidResultException : Exception
    {
        public ForbidResultException()
           : base(StatusCodes.Status403Forbidden.ToString())
        {
        }
    }
}
