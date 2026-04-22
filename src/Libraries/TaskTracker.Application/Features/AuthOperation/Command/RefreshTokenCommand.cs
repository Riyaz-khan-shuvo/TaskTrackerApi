using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.AuthOperation.ViewModels;
using TaskTracker.Application.Interfaces.Services;

namespace TaskTracker.Application.Features.AuthOperation.Command
{
    public class RefreshTokenCommand : RefreshTokenDto, IRequest<ResultVM>
    {

        public class Handler : IRequestHandler<RefreshTokenCommand, ResultVM>
        {
            private readonly IJwtService _jwtService;

            public Handler(IJwtService jwtService)
            {
                _jwtService = jwtService;
            }

            public Task<ResultVM> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var newToken = _jwtService.RefreshToken(request.Token);

                    return Task.FromResult(new ResultVM
                    {
                        Status = "Success",
                        Message = "Token refreshed successfully.",
                        Data = new { Token = newToken }
                    });
                }
                catch (Exception ex)
                {
                    return Task.FromResult(new ResultVM
                    {
                        Status = "Fail",
                        Message = ex.Message
                    });
                }
            }
        }
    }
}
