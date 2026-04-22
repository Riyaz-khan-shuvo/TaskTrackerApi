using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.AuthOperation.ViewModels;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class ResetPasswordCommand : ResetPasswordVM, IRequest<ResultVM>
    {
        //public string Email { get; set; }
        //public string Token { get; set; }
        //public string NewPassword { get; set; }

        public class Handler : IRequestHandler<ResetPasswordCommand, ResultVM>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<ResultVM> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return new ResultVM { Status = "false", Message = "Invalid request." };
                }

                var decodedToken = HttpUtility.UrlDecode(request.Token);
                var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

                if (!result.Succeeded)
                {
                    return new ResultVM
                    {
                        Status = "false",
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                return new ResultVM { Status = "Success", Message = "Password reset successfully." };
            }
        }
    }
}
