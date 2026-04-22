using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Web;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class ForgotPasswordCommand : IRequest<ResultVM>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<ForgotPasswordCommand, ResultVM>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailService _emailService;
            private readonly IConfiguration _configuration;

            public Handler(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration configuration)
            {
                _userManager = userManager;
                _emailService = emailService;
                _configuration = configuration;
            }

            public async Task<ResultVM> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !user.EmailConfirmed)
                {
                    // Do not reveal if email exists
                    return new ResultVM { Status = "Success", Message = "If an account with this email exists, a reset link has been sent." };
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = HttpUtility.UrlEncode(token);

                var frontendUrl = _configuration["Frontend:Url"];
                var resetLink = $"{frontendUrl}/Account/ResetPassword?email={HttpUtility.UrlEncode(user.Email)}&token={encodedToken}";

                var emailContent = $@"
                    <p>Hello {user.FirstName},</p>
                    <p>Click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>Reset Password</a></p>
                    <p>This link will expire in 24 hours.</p>";

                await _emailService.SendEmailAsync(user.Email, "EcoBazar Password Reset", emailContent);

                return new ResultVM { Status = "Success", Message = "If an account with this email exists, a reset link has been sent." };
            }
        }
    }
}
