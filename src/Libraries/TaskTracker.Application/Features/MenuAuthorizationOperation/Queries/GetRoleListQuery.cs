using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Queries
{
    public class GetRoleListQuery : IRequest<ResultVM>
    {
        public CommonVM Dto { get; set; }
        public string[] ConditionalFields { get; set; }
        public string[] ConditionalValues { get; set; }

        public GetRoleListQuery(CommonVM dto, string[] conditionalFields, string[] conditionalValues)
        {
            Dto = dto;
            ConditionalFields = conditionalFields;
            ConditionalValues = conditionalValues;
        }

        internal class Handler : IRequestHandler<GetRoleListQuery, ResultVM>
        {
            private readonly IMenuAuthorizationRepository _menuAuthorizationRepository;

            public Handler(IMenuAuthorizationRepository menuAuthorizationRepository)
            {
                _menuAuthorizationRepository = menuAuthorizationRepository;
            }

            public async Task<ResultVM> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
            {
                var data = await _menuAuthorizationRepository.GetRoleListData(
                            request.Dto,
                            request.ConditionalFields,
                            request.ConditionalValues
                        );
                return new ResultVM
                {
                    Status = "Success",
                    Message = "Role list retrieved successfully.",
                    Data = data,
                    Count = data.Count
                };

            }
        }
    }
}
