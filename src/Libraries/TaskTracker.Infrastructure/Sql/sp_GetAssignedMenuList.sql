Create PROCEDURE [dbo].[sp_GetAssignedMenuList]
    @calltype VARCHAR(50),
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF @calltype = 'get_List'
    BEGIN

        SELECT
            RM.MenuId,
            M.Url,
            M.Name AS MenuName,
            M.IconClass,
            M.Controller,
            ISNULL(M.ParentId, 0) AS ParentId,
            ISNULL(M.SubParentId, 0) AS SubParentId,
            ISNULL(M.SubChildId, 0) AS SubChildId,
            ISNULL(M.DisplayOrder, 0) AS DisplayOrder,

            MAX(CAST(RM.[List] AS INT))   AS [List],
            MAX(CAST(RM.[Insert] AS INT)) AS [Insert],
            MAX(CAST(RM.[Post] AS INT))   AS [Post],
            MAX(CAST(RM.[Delete] AS INT)) AS [Delete],

            (
                SELECT COUNT(DISTINCT MC.Id)
                FROM [TaskTrackerDb].[dbo].[Menus] MC
                INNER JOIN [TaskTrackerDb].[dbo].[RoleMenus] RMC
                    ON MC.Id = RMC.MenuId
                INNER JOIN [TaskTrackerDb].[dbo].[Roles] RC
                    ON RMC.RoleId = RC.Id
                INNER JOIN [TaskTrackerAuthDb].[dbo].[UserRoles] URC
                    ON RC.IdentityRoleId = URC.RoleId
                WHERE MC.ParentId = M.Id
                  AND MC.IsActive = 1
                  AND URC.UserId = @UserId
            ) AS TotalChild

        FROM [TaskTrackerAuthDb].[dbo].[UserRoles] UR
        INNER JOIN [TaskTrackerDb].[dbo].[Roles] R
            ON UR.RoleId = R.IdentityRoleId
        INNER JOIN [TaskTrackerDb].[dbo].[RoleMenus] RM
            ON R.Id = RM.RoleId
        INNER JOIN [TaskTrackerDb].[dbo].[Menus] M
            ON RM.MenuId = M.Id

        WHERE UR.UserId = @UserId
          AND M.IsActive = 1

        GROUP BY
            RM.MenuId,
            M.Url,
            M.Name,
            M.IconClass,
            M.Controller,
            M.ParentId,
            M.SubParentId,
            M.SubChildId,
            M.DisplayOrder,
            M.Id

        ORDER BY M.DisplayOrder;
    END
    ELSE
    BEGIN
        RAISERROR(@calltype, 16, 1);
    END
END