using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Features.UserMenuOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.Interfaces.Services.Common;
using TaskTracker.Core.Entities;
using TaskTracker.Infrastructure.Persistence.DataContext;

namespace TaskTracker.Infrastructure.Persistence.Repositories
{
    public class UserMenuRepository : IUserMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthDbContext _authContext;
        private readonly IGridService<UserVM> _gridService;
        private readonly IDbConnectionFactory _connectionFactory;

        public UserMenuRepository(ApplicationDbContext context, IGridService<UserVM> gridService, IDbConnectionFactory connectionFactory, AuthDbContext authContext)
        {
            _context = context;
            _gridService = gridService;
            _connectionFactory = connectionFactory;
            _authContext = authContext;
        }

        public Task<UserMenu> GetByUserAndMenuAsync(Guid userId, int menuId)
        {
            throw new NotImplementedException();
        }

        public Task<Menu> GetMenuByControllerAsync(string controllerName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserMenu>> GetMenusByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<GridEntity<UserVM>> UserGridDataAsync(GridOptions options)
        {
            var filterCondition = options.filter?.Filters.Count > 0
                ? " AND (" + GridQueryBuilder<UserVM>.FilterCondition(options.filter) + ")"
                : string.Empty;

            var sortColumn = options.sort?.Count > 0
                ? options.sort[0].field + " " + options.sort[0].dir
                : "u.Id DESC";

            string sqlQuery = $@"
        -- Count query
        SELECT COUNT(DISTINCT u.Id) AS totalcount
        FROM Users u
        LEFT JOIN UserRoles ur ON u.Id = ur.UserId
        LEFT JOIN Roles r ON ur.RoleId = r.Id
        WHERE 1 = 1
        {filterCondition}

        -- Data query
        SELECT *
        FROM (
            SELECT 
                ROW_NUMBER() OVER(ORDER BY {sortColumn}) AS rowindex,
                u.Id AS UserId,
                u.FirstName,
                u.LastName,
                ISNULL(ur.RoleId, 0) AS RoleId,
                r.Name AS RoleName,
                u.Email
            FROM Users u
            LEFT JOIN UserRoles ur ON u.Id = ur.UserId
            LEFT JOIN Roles r ON ur.RoleId = r.Id
            WHERE 1 = 1
            {filterCondition}
        ) AS a
        WHERE rowindex > @skip AND (@take = 0 OR rowindex <= @take)
    ";

            var data = await _gridService.GetAuthGridDataCmdAsync(
                options,
                sqlQuery,
                "u.Id"
            );

            return data;
        }

    }
}
