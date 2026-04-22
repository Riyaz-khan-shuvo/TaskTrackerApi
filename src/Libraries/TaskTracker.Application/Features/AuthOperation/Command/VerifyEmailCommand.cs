using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.AuthOperation.ViewModels;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class VerifyEmailCommand : VerifyEmailVM, IRequest<ResultVM>
    {
        public class Handler : IRequestHandler<VerifyEmailCommand, ResultVM>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<ResultVM> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ResultVM
                    {
                        Status = "Fail",
                        Message = "Invalid user ID."
                    };
                }
                var result = await _userManager.ConfirmEmailAsync(user, request.Token);
                if (!result.Succeeded)
                {
                    return new ResultVM
                    {
                        Status = "Fail",
                        Message = "Invalid or expired verification link."
                    };
                }
                return new ResultVM
                {
                    Status = "Success",
                    Message = "Email verified successfully!"
                };
            }
        }
    }
}
