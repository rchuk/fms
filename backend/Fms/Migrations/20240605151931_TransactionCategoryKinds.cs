using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fms.Migrations
{
    /// <inheritdoc />
    public partial class TransactionCategoryKinds : Migration
    {
        private readonly static string[] EnumVariants = ["INCOME", "EXPENSE", "MIXED"];
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "TransactionCategoryKinds",
                new []{ "Name" }, 
                ArrayToColumn(EnumVariants)
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var variants = String.Join(", ", EnumVariants);

            migrationBuilder.Sql($"DELETE FROM TransactionCategoryKinds WHERE Name IN ({variants})");
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
