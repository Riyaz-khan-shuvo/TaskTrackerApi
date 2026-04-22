using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.Features.UserMenuOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Core.Entities;

namespace TaskTracker.Application.Interfaces.Repositories
{
    public interface IMenuAuthorizationRepository : IGenericRepository<Role, int>
    {
        Task<GridEntity<UserRoleVM>> GetRoleGridData(GridOptions options);
        Task<GridEntity<UserVM>> UserGridDataAsync(GridOptions options);

        Task<List<RoleMenuVM>> GetMenuAccessData(CommonVM vm);
        Task<List<Role>> GetRoleListData(CommonVM vm, string[] conditionalFields, string[] conditionalValues);
        Task<RoleMenu> InsertRoleMenu(RoleMenu entity, CancellationToken cancellationToken = default);
        Task<UserMenu> InsertUserMenu(UserMenu entity, CancellationToken cancellationToken = default);
        Task<UserMenu> UpdateUserMenu(UserMenu entity, CancellationToken cancellationToken = default);
        Task DeleteRoleMenusByRoleId(int roleId, CancellationToken cancellationToken = default);
        Task DeleteUserMenusByUserId(Guid userId, CancellationToken cancellationToken);
    }
}
