using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class LogoutCommand : IRequest<ResultVM>
    {
        public class Handler : IRequestHandler<LogoutCommand, ResultVM>
        {
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
            {
                _signInManager = signInManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<ResultVM> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                await _signInManager.SignOutAsync();

                var context = _httpContextAccessor.HttpContext;
                if (context != null)
                {
                    foreach (var cookie in context.Request.Cookies.Keys)
                    {
                        context.Response.Cookies.Delete(cookie);
                    }
                }


                return new ResultVM
                {
                    Status = "Success",
                    Message = "Logged out successfully."
                };
            }
        }
    }
}
