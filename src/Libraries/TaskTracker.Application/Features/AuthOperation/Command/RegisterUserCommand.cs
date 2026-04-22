using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Web;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.AuthOperation.ViewModels;
using TaskTracker.Application.Helpers;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Application.Validators;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class RegisterUserCommand : RegisterVM, IRequest<ResultVM>
    {
        public class Validator : BaseValidator<RegisterUserCommand>
        {
            public Validator()
            {
                ValidateName(x => x.FirstName, "First Name");
                ValidateEmail(x => x.Email);
                ValidatePassword(x => x.Password);
            }
        }

        public class Handler : IRequestHandler<RegisterUserCommand, ResultVM>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailService _emailService;
            private readonly IConfiguration _configuration;

            public Handler(
                UserManager<ApplicationUser> userManager,
                IEmailService emailService,
                IConfiguration configuration)
            {
                _userManager = userManager;
                _emailService = emailService;
                _configuration = configuration;
            }

            public async Task<ResultVM> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                    return new ResultVM { Status = "Fail", Message = "This email is already registered." };
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new ResultVM
                    {
                        Status = "Fail",
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = HttpUtility.UrlEncode(token);

                var frontendUrl = _configuration["Frontend:Url"];
                var verificationLink = $"{frontendUrl}/Account/VerifyEmail?userId={user.Id}&token={encodedToken}";
                var emailContent = EmailTemplates.GetVerificationEmail(user.FirstName, verificationLink, _configuration);
                await _emailService.SendEmailAsync(user.Email, "Verify Your Email - TaskTracker", emailContent);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "User registered successfully. Please check your email to verify your account."
                };
            }
        }
    }
}
