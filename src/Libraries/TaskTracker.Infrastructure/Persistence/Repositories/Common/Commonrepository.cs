using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.Interfaces.Services.Common;

namespace TaskTracker.Infrastructure.Persistence.Repositories.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly ICommonService _commonService;
        public CommonRepository(ICommonService commonService)
        {
            _commonService = commonService;
        }
        public Task<List<UserMenuVM>> GetAssignedMenuListAsync(Guid userId)
        {
            return _commonService.SelectDataListAsync<UserMenuVM>("sp_GetAssignedMenuList", "get_List", new { UserId = userId });
        }
    }
}
