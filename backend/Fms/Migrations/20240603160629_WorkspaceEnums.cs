using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fms.Migrations
{
    /// <inheritdoc />
    public partial class WorkspaceEnums : Migration
    {
        private readonly static string[] WorkspaceKinds = ["PRIVATE", "SHARED"];
        private readonly static string[] WorkspaceRoles = ["OWNER", "ADMIN", "COLLABORATOR", "VIEWER"];
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "WorkspaceKinds",
                new[] { "Name" },
                ArrayToColumn(WorkspaceKinds)
            );
            
            migrationBuilder.InsertData(
                "WorkspaceRoles",
                new[] { "Name" },
                ArrayToColumn(WorkspaceRoles)
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var kindsStr = String.Join(", ", WorkspaceKinds);
            var rolesStr = String.Join(", ", WorkspaceRoles);

            migrationBuilder.Sql($"DELETE FROM WorkspaceKinds WHERE Name IN ({kindsStr})");
            migrationBuilder.Sql($"DELETE FROM WorkspaceRoles WHERE Name IN ({rolesStr})");
        }
        
        public static object[,] ArrayToColumn(object[] array)
        {
            var result = new object[array.Length, 1];
            for (int i = 0; i < array.Length; ++i)
                result[i, 0] = array[i];
            
            return result;
        }
    }
}
