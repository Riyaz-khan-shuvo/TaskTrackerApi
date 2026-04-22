using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.Features.UserMenuOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.Interfaces.Services.Common;
using TaskTracker.Core.Entities;
using TaskTracker.Infrastructure.Persistence.DataContext;
using TaskTracker.Infrastructure.Persistence.Repositories.Common;

namespace TaskTracker.Infrastructure.Persistence.Repositories
{
    public class MenuAuthorizationRepository : GenericRepository<Role, int>, IMenuAuthorizationRepository
    {
        private readonly IGridService<UserRoleVM> _gridService;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IGridService<UserVM> _userGridService;


        public MenuAuthorizationRepository(ApplicationDbContext context, IGridService<UserRoleVM> gridService, IDbConnectionFactory connectionFactory, IGridService<UserVM> userGridService) : base(context)
        {

            _gridService = gridService;
            _connectionFactory = connectionFactory;
            _userGridService = userGridService;
        }

        public async Task<GridEntity<UserRoleVM>> GetRoleGridData(GridOptions options)
        {
            var query = _context.Roles
                .AsNoTracking()
                .Select(r => new UserRoleVM
                {
                    Id = r.Id,
                    Name = r.Name
                });

            var totalCount = await query.CountAsync();

            var pagedData = await query
                .Skip(options.skip)
                .Take(options.take)
                .ToListAsync();

            var result = new GridResult<UserRoleVM>().Data(pagedData, totalCount);
            return result;
        }

        public async Task<List<RoleMenuVM>> GetMenuAccessData(CommonVM vm)
        {
            string sqlQuery = @"
        DECLARE @Id INT = @RoleId;

        WITH MenuHierarchy AS (
            SELECT 
                Id, 
                [Name], 
                ParentId, 
                CAST([Name] AS NVARCHAR(MAX)) AS MenuName
            FROM Menus
            WHERE IsActive = 1 AND ParentId = 0

            UNION ALL

            SELECT 
                m.Id, 
                m.[Name], 
                m.ParentId, 
                CAST(mc.MenuName + ' > ' + m.[Name] AS NVARCHAR(MAX)) AS MenuName
            FROM Menus m
            INNER JOIN MenuHierarchy mc ON m.ParentId = mc.Id
            WHERE m.IsActive = 1
        )

        SELECT 
            M.Id AS MenuId,
            CASE WHEN RM.MenuId IS NOT NULL THEN 1 ELSE 0 END AS IsChecked,
            ISNULL(RM.RoleId, 0) AS RoleId,
            M.ParentId,
            ISNULL(M.Url, '') AS Url,
            MH.MenuName,
            ISNULL(M.Controller, '') AS Controller,
            M.DisplayOrder,
            ISNULL(RM.List, 0) AS [List],
            ISNULL(RM.[Insert], 0) AS [Insert],
            ISNULL(RM.[Delete], 0) AS [Delete],
            ISNULL(RM.Post, 0) AS Post
        FROM Menus M
        JOIN MenuHierarchy MH ON M.Id = MH.Id
        LEFT JOIN RoleMenus RM ON RM.MenuId = M.Id AND RM.RoleId = @Id
        WHERE M.IsActive = 1
        ORDER BY M.DisplayOrder;
    ";

            using var connection = _connectionFactory.CreateDefaultConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@RoleId", vm.IntId);

            var menuData = await connection.QueryAsync<RoleMenuVM>(sqlQuery, parameters, commandType: CommandType.Text);
            return menuData.ToList();
        }

        public async Task<List<Role>> GetRoleListData(CommonVM vm, string[] conditionalFields, string[] conditionalValues)
        {
            var query = _context.Roles
                .AsNoTracking()
                .AsQueryable();

            if (vm != null)
            {
                if (vm.IntId != 0)
                    query = query.Where(b => b.Id == vm.IntId);
            }

            var list = await query
                .Select(b => new Role
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToListAsync();

            return list;
        }

        public async Task<RoleMenu> InsertRoleMenu(RoleMenu entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<RoleMenu>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<UserMenu> InsertUserMenu(UserMenu entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<UserMenu>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<UserMenu> UpdateUserMenu(UserMenu entity, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<UserMenu>();

            var existing = await dbSet
                .FirstOrDefaultAsync(x => x.UserId == entity.UserId && x.MenuId == entity.MenuId, cancellationToken);

            if (existing != null)
                dbSet.Remove(existing);

            await dbSet.AddAsync(entity, cancellationToken);

            return entity;
        }

        public async Task DeleteRoleMenusByRoleId(int roleId, CancellationToken cancellationToken = default)
        {
            var items = await _context.Set<RoleMenu>()
                .Where(x => x.RoleId == roleId)
                .ToListAsync(cancellationToken);

            if (items.Any())
                _context.Set<RoleMenu>().RemoveRange(items);
        }

        public async Task DeleteUserMenusByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var userMenus = await _context.UserMenus
                .Where(um => um.UserId == userId)
                .ToListAsync(cancellationToken);

            if (userMenus.Any())
            {
                _context.UserMenus.RemoveRange(userMenus);
            }
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
        left outer join UserRoles ur ON u.Id = ur.UserId
        left outer join Roles r ON ur.RoleId = r.Id
        WHERE 1 = 1
AND (r.Name IS NULL OR r.Name <> 'User') 
        {filterCondition}

        -- Data query
        SELECT *
        FROM (
            SELECT 
                ROW_NUMBER() OVER(ORDER BY {sortColumn}) AS rowindex,
                u.Id,
                u.FirstName,
                u.LastName,
                ur.RoleId AS RoleId,
                r.Name AS RoleName,
                u.Email
            FROM Users u
            left outer join UserRoles ur ON u.Id = ur.UserId
            left outer join Roles r ON ur.RoleId = r.Id
            WHERE 1 = 1
      AND (r.Name IS NULL OR r.Name <> 'User') 

            {filterCondition}
        ) AS a
        WHERE rowindex > @skip AND (@take = 0 OR rowindex <= @take)
    ";

            var data = await _userGridService.GetAuthGridDataCmdAsync(
                options,
                sqlQuery,
                "u.Id"
            );

            return data;
        }
    }
}
