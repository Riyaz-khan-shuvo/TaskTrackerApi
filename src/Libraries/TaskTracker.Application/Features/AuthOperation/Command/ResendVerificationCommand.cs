using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Web;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class ResendVerificationCommand : IRequest<ResultVM>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<ResendVerificationCommand, ResultVM>
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

            public async Task<ResultVM> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user.EmailConfirmed)
                    return new ResultVM { Status = "Fail", Message = "Email is already verified." };

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = HttpUtility.UrlEncode(token);

                var frontendUrl = _configuration["Frontend:Url"];
                var verificationLink = $"{frontendUrl}/Account/VerifyEmail?userId={user.Id}&token={encodedToken}";

                var emailContent = $@"
    <p>Hello {user.FirstName},</p>
    <p>You requested to resend the verification email. Click the link below to verify your account:</p>
    <p><a href='{verificationLink}'>Verify Email</a></p>
    <p>This link will expire in 24 hours.</p>";
                await _emailService.SendEmailAsync(user.Email, "EcoBazar Email Verification", emailContent);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Verification email sent successfully. Please check your inbox."
                };


            }
        }
    }
}
