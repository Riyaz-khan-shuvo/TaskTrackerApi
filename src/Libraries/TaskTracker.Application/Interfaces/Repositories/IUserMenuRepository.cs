using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Features.UserMenuOperation.ViewModels;
using TaskTracker.Core.Entities;

namespace TaskTracker.Application.Interfaces.Repositories
{
    public interface IUserMenuRepository
    {
        Task<GridEntity<UserVM>> UserGridDataAsync(GridOptions options);

        Task<IEnumerable<UserMenu>> GetMenusByUserIdAsync(Guid userId);
        Task<UserMenu> GetByUserAndMenuAsync(Guid userId, int menuId);
        Task<Menu> GetMenuByControllerAsync(string controllerName);
    }
}
