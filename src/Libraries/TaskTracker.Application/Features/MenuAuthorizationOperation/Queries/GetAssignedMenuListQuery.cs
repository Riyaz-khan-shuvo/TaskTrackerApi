using MediatR;
using Microsoft.Extensions.Caching.Memory;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories.Common;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Queries
{
    public class GetAssignedMenuListQuery : IRequest<ResultVM>
    {
        public Guid UserId { get; set; }

        public class Handler : IRequestHandler<GetAssignedMenuListQuery, ResultVM>
        {
            private readonly ICommonRepository _commonRepository;
            private readonly IMemoryCache _cache;

            public Handler(ICommonRepository commonRepository, IMemoryCache cache = null)
            {
                _commonRepository = commonRepository;
                _cache = cache;
            }

            public async Task<ResultVM> Handle(GetAssignedMenuListQuery request, CancellationToken cancellationToken)
            {
                var cacheKey = $"UserMenu_{request.UserId}";

                if (_cache.TryGetValue(cacheKey, out Dictionary<string, UserMenuVM>? menuDict))
                {
                    var menuList = menuDict.Values.ToList();

                    return new ResultVM
                    {
                        Status = "Success",
                        Message = "Assigned menu list retrieved successfully.",
                        Data = menuList,
                        Count = menuList.Count
                    };
                }
                else
                {
                    var userMenu = await _commonRepository.GetAssignedMenuListAsync(request.UserId);

                    return new ResultVM
                    {
                        Status = "Success",
                        Message = "Assigned menu list retrieved successfully.",
                        Data = userMenu,
                        Count = userMenu?.Count ?? 0
                    };
                }

                //try
                //{
                //    var menuList = await _commonRepository.GetAssignedMenuListAsync(request.UserId);

                //    return new ResultVM
                //    {
                //        Status = "Success",
                //        Message = "Assigned menu list retrieved successfully.",
                //        Data = menuList,
                //        Count = menuList?.Count ?? 0
                //    };
                //}
                //catch (Exception ex)
                //{
                //    return new ResultVM
                //    {
                //        Status = "Fail",
                //        Message = "An error occurred while retrieving assigned menu list.",
                //        ExMessage = ex.Message
                //    };
                //}
            }
        }
    }
}
