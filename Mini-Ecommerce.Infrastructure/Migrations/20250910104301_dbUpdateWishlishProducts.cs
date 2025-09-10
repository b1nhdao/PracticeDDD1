using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dbUpdateWishlishProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wishlish_products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    product_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    product_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    date_added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlish_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_wishlish_products_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_wishlish_products_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_wishlish_products_CustomerId",
                table: "wishlish_products",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_wishlish_products_ProductId",
                table: "wishlish_products",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wishlish_products");
        }
    }
}
