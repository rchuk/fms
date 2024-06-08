using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fms.Migrations
{
    /// <inheritdoc />
    public partial class Subscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionKindId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriptionKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionKinds", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionKindId",
                table: "Users",
                column: "SubscriptionKindId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SubscriptionKinds_SubscriptionKindId",
                table: "Users",
                column: "SubscriptionKindId",
                principalTable: "SubscriptionKinds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SubscriptionKinds_SubscriptionKindId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "SubscriptionKinds");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionKindId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionKindId",
                table: "Users");
        }
    }
}
