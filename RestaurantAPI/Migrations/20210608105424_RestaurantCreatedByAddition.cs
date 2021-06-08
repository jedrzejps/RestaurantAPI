using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantAPI.Migrations
{
    public partial class RestaurantCreatedByAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CreatedById",
                table: "Restaurants",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_CreatedById",
                table: "Restaurants",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_CreatedById",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_CreatedById",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "RoleID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                newName: "IX_Users_RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
