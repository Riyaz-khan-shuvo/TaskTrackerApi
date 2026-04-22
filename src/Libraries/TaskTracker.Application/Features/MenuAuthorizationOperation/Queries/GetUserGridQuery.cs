using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Queries
{
    public class GetUserGridQuery : GridOptions, IRequest<ResultVM>
    {

        public class Handler : IRequestHandler<GetUserGridQuery, ResultVM>
        {
            private readonly IMenuAuthorizationRepository _menuAuthorizationRepository;

            public Handler(IMenuAuthorizationRepository menuAuthorizationRepository)
            {
                _menuAuthorizationRepository = menuAuthorizationRepository;
            }

            public async Task<ResultVM> Handle(GetUserGridQuery request, CancellationToken cancellationToken)
            {
                var gridData = await _menuAuthorizationRepository.UserGridDataAsync(request);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Role data retrieved successfully.",
                    Data = gridData,
                    Count = gridData.TotalCount
                };
            }
        }
    }
}
