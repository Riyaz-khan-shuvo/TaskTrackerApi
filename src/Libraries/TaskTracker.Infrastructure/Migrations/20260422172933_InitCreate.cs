using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessCategoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCategoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SubParentId = table.Column<int>(type: "int", nullable: true),
                    SubChildId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateFrom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateFrom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessCategoryTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessCategories_BusinessCategoryTypes_BusinessCategoryTypeId",
                        column: x => x.BusinessCategoryTypeId,
                        principalTable: "BusinessCategoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    List = table.Column<bool>(type: "bit", nullable: false),
                    Insert = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    Post = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateFrom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    MenuId = table.Column<int>(type: "int", nullable: true),
                    List = table.Column<bool>(type: "bit", nullable: false),
                    Insert = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    Post = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdateFrom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PriorityId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_BusinessCategories_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "BusinessCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BusinessCategoryTypes",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[] { 1, "Task Priority Group", true, "Priority" });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Controller", "CreatedBy", "CreatedFrom", "CreatedOn", "DisplayOrder", "IconClass", "IsActive", "LastModifiedBy", "LastModifiedOn", "LastUpdateFrom", "Module", "Name", "ParentId", "SubChildId", "SubParentId", "Url" },
                values: new object[,]
                {
                    { 1, null, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 8, "nav-icon fa-solid fa-user-shield", true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SetUp", "Menu Authorization", 0, 0, 0, null },
                    { 2, "MenuAuthorization", "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 9, "nav-icon fas fa-user-circle", true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SetUp", "Role", 1, 0, 0, "/SetUp/MenuAuthorization/Role" },
                    { 3, "MenuAuthorization", "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 11, "nav-icon fas fa-user-shield", true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SetUp", "Role Menu Assign", 1, 0, 0, "/SetUp/MenuAuthorization/RoleMenu" },
                    { 4, "Task", "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 12, "nav-icon fas fa-tasks", true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SetUp", "Task", 1, 0, 0, "/SetUp/Task" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedBy", "CreatedFrom", "CreatedOn", "IdentityRoleId", "LastModifiedBy", "LastModifiedOn", "LastUpdateFrom", "Name" },
                values: new object[,]
                {
                    { 1, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0"), "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SuperAdmin" },
                    { 2, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("37cc67e1-41ca-461c-bf34-2b5e62dbae32"), "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Admin" },
                    { 3, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("a0cab2c3-6558-4a1c-be81-dfb39180da3d"), "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "User" }
                });

            migrationBuilder.InsertData(
                table: "BusinessCategories",
                columns: new[] { "Id", "BusinessCategoryTypeId", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Low Priority", true, "Low" },
                    { 2, 1, "Medium Priority", true, "Medium" },
                    { 3, 1, "High Priority", true, "High" }
                });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "Id", "CreatedBy", "CreatedFrom", "CreatedOn", "Delete", "Insert", "LastModifiedBy", "LastModifiedOn", "LastUpdateFrom", "List", "MenuId", "Post", "RoleId" },
                values: new object[,]
                {
                    { 1, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, 1, true, 1 },
                    { 2, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, 2, true, 1 },
                    { 3, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, 3, true, 1 },
                    { 4, "SYSTEM", null, new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), true, true, "SYSTEM", new DateTimeOffset(new DateTime(2024, 11, 18, 10, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, 4, true, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategories_BusinessCategoryTypeId",
                table: "BusinessCategories",
                column: "BusinessCategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Name",
                table: "Menus",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_MenuId",
                table: "RoleMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PriorityId",
                table: "Tasks",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMenus_MenuId",
                table: "UserMenus",
                column: "MenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMenus");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "UserMenus");

            migrationBuilder.DropTable(
                name: "BusinessCategories");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "BusinessCategoryTypes");
        }
    }
}
