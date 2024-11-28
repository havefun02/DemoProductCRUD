using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoCRUD.Migrations
{
    public partial class sp_GetProductById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var command = @"
            CREATE PROCEDURE sp_GetProductById(IN Id INT)
            BEGIN
                SELECT ProductId, ProductName, Price, StockQuantity
                FROM Products
                WHERE ProductId = Id;
            END;";
            migrationBuilder.Sql(command);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetProductById;");
        }


    }
}


