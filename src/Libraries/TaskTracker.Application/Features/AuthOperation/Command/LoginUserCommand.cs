using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.AuthOperation.ViewModels;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Application.Validators;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class LoginUserCommand : LoginVM, IRequest<ResultVM>
    {
        public class Validator : BaseValidator<LoginUserCommand>
        {
            public Validator()
            {
                ValidateEmail(x => x.Email);
                ValidatePassword(x => x.Password);
            }
        }
        public class Handler : IRequestHandler<LoginUserCommand, ResultVM>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IJwtService _jwtService;

            public Handler(
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IJwtService jwtService)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtService = jwtService;
            }

            public async Task<ResultVM> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return new ResultVM
                    {
                        Status = "Fail",
                        Message = "We couldn’t find an account with this email. Please check your email or sign up."
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                    return new ResultVM { Status = "Fail", Message = "Incorrect password. Please try again." };

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(
                    user.Id,
                    user.UserName,
                    user.FirstName,
                    user.LastName,
                    roles.FirstOrDefault() ?? "Customer"
                );

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Login successful.",
                    Data = new { token }
                };
            }
        }
    }
}
