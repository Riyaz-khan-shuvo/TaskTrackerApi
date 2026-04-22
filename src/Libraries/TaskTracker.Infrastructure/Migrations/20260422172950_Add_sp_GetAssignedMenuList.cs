using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Infrastructure.Migrations
{
    public partial class Add_sp_GetAssignedMenuList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                "Sql",
                "sp_GetAssignedMenuList.sql"
            );

            var sql = File.ReadAllText(path);

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS sp_GetAssignedMenuList
            ");
        }
    }
}
