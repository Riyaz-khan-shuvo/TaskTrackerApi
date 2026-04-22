using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Queries
{
    public class GetMenuAccessDataQuery : IRequest<ResultVM>
    {
        public CommonVM VM { get; set; }
        public GetMenuAccessDataQuery(CommonVM vm)
        {
            VM = vm;
        }


        public class Handler : IRequestHandler<GetMenuAccessDataQuery, ResultVM>
        {
            private readonly IMenuAuthorizationRepository _menuAuthorizationRepository;

            public Handler(IMenuAuthorizationRepository menuAuthorizationRepository)
            {
                _menuAuthorizationRepository = menuAuthorizationRepository;
            }

            public async Task<ResultVM> Handle(GetMenuAccessDataQuery request, CancellationToken cancellationToken)
            {
                var menuData = await _menuAuthorizationRepository.GetMenuAccessData(request.VM);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Menu access data loaded successfully.",
                    Data = menuData,
                    Count = menuData?.Count() ?? 0
                };
            }
        }
    }
}
