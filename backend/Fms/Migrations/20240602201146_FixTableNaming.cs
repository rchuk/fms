using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fms.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToUserEntity_OrganizationRoles_RoleId",
                table: "OrganizationToUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToUserEntity_Organizations_OrganizationId",
                table: "OrganizationToUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToUserEntity_Users_UserId",
                table: "OrganizationToUserEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationToUserEntity",
                table: "OrganizationToUserEntity");

            migrationBuilder.RenameTable(
                name: "OrganizationToUserEntity",
                newName: "OrganizationToUser");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToUserEntity_UserId",
                table: "OrganizationToUser",
                newName: "IX_OrganizationToUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToUserEntity_RoleId",
                table: "OrganizationToUser",
                newName: "IX_OrganizationToUser_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationToUser",
                table: "OrganizationToUser",
                columns: new[] { "OrganizationId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToUser_OrganizationRoles_RoleId",
                table: "OrganizationToUser",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToUser_Organizations_OrganizationId",
                table: "OrganizationToUser",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToUser_Users_UserId",
                table: "OrganizationToUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToUser_OrganizationRoles_RoleId",
                table: "OrganizationToUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToUser_Organizations_OrganizationId",
                table: "OrganizationToUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToUser_Users_UserId",
                table: "OrganizationToUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationToUser",
                table: "OrganizationToUser");

            migrationBuilder.RenameTable(
                name: "OrganizationToUser",
                newName: "OrganizationToUserEntity");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToUser_UserId",
                table: "OrganizationToUserEntity",
                newName: "IX_OrganizationToUserEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToUser_RoleId",
                table: "OrganizationToUserEntity",
                newName: "IX_OrganizationToUserEntity_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationToUserEntity",
                table: "OrganizationToUserEntity",
                columns: new[] { "OrganizationId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToUserEntity_OrganizationRoles_RoleId",
                table: "OrganizationToUserEntity",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToUserEntity_Organizations_OrganizationId",
                table: "OrganizationToUserEntity",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToUserEntity_Users_UserId",
                table: "OrganizationToUserEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
