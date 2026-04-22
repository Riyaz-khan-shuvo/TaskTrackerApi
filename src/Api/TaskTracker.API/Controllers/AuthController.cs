using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.AuthOperation.Command;

namespace TaskTracker.API.Controllers
{

    public class AuthController : BaseController
    {

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ResultVM> Register([FromBody] RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<ResultVM> VerifyEmail(VerifyEmailCommand command)
        {
            return await Mediator.Send(command);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("resend-verification")]
        [AllowAnonymous]
        public async Task<ResultVM> ResendVerification([FromBody] ResendVerificationCommand command)
        {
            return await Mediator.Send(command);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ResultVM> Login([FromBody] LoginUserCommand command)
        {
            return await Mediator.Send(command);
        }
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ResultVM> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ResultVM> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            return await Mediator.Send(command);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ResultVM> Refresh([FromBody] RefreshTokenCommand command)
        {
            return await Mediator.Send(command);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("logout")]
        [Authorize]
        public async Task<ResultVM> Logout()
        {
            return await Mediator.Send(new LogoutCommand());
        }
    }
}
