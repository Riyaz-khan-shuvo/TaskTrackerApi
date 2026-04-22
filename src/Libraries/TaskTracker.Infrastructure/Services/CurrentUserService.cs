using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskTracker.Application.Interfaces.Services;

namespace TaskTracker.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");

            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue("name");
            //Roles = httpContextAccessor.HttpContext?.User?.FindFirstValue("role").Split(",");
            IsAuthenticated = httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated ?? false;
            AuthenticationToken = httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
            //AuthenticationToken = httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
        }

        public string UserId { get; }

        public bool IsAuthenticated { get; }

        public string Username { get; }

        public string[] Roles { get; }

        public string AuthenticationToken { get; }

        //public string CurrentUserBusinessProfileId { get; }

    }

}
