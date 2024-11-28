using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoCRUD.Migrations
{
    public partial class sp_GetAllProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var command = @"
            CREATE PROCEDURE sp_GetAllProducts()
            BEGIN
                SELECT ProductId, ProductName, Price, StockQuantity FROM Products;
            END;";

            migrationBuilder.Sql(command);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetAllProducts;");
        }
    }
}
