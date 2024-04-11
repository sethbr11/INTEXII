using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INTEXII.Migrations.IntexW24dataset
{
    /// <inheritdoc />
    public partial class Stable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    customer_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", nullable: false),
                    last_name = table.Column<string>(type: "TEXT", nullable: false),
                    birth_date = table.Column<string>(type: "TEXT", nullable: false),
                    country_of_residence = table.Column<string>(type: "TEXT", nullable: true),
                    gender = table.Column<string>(type: "TEXT", nullable: true),
                    age = table.Column<double>(type: "INTEGER", nullable: true),
                    username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.customer_ID);
                });

            migrationBuilder.CreateTable(
                name: "LineItems",
                columns: table => new
                {
                    transaction_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    product_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    qty = table.Column<int>(type: "INTEGER", nullable: false),
                    rating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItems", x => new { x.transaction_ID, x.product_ID });
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    transaction_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    customer_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<string>(type: "TEXT", nullable: false),
                    day_of_week = table.Column<string>(type: "TEXT", nullable: false),
                    time = table.Column<int>(type: "INTEGER", nullable: false),
                    entry_mode = table.Column<string>(type: "TEXT", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    type_of_transaction = table.Column<string>(type: "TEXT", nullable: false),
                    country_of_transaction = table.Column<string>(type: "TEXT", nullable: false),
                    shipping_address = table.Column<string>(type: "TEXT", nullable: false),
                    bank = table.Column<string>(type: "TEXT", nullable: false),
                    type_of_card = table.Column<string>(type: "TEXT", nullable: false),
                    fraud = table.Column<int>(type: "INTEGER", nullable: false),
                    pred_fraud = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.transaction_ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    product_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    year = table.Column<int>(type: "NUMERIC", nullable: false),
                    num_parts = table.Column<int>(type: "NUMERIC", nullable: false),
                    price = table.Column<int>(type: "NUMERIC", nullable: false),
                    img_link = table.Column<string>(type: "TEXT", nullable: true),
                    primary_color = table.Column<string>(type: "TEXT", nullable: true),
                    secondary_color = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    category = table.Column<string>(type: "TEXT", nullable: true),
                    PublicCategory = table.Column<string>(type: "TEXT", nullable: true),
                    recommendation1 = table.Column<int>(type: "NUMERIC", nullable: true),
                    similarity1 = table.Column<double>(type: "NUMERIC", nullable: true),
                    recommendation2 = table.Column<int>(type: "NUMERIC", nullable: true),
                    similarity2 = table.Column<double>(type: "NUMERIC", nullable: true),
                    recommendation3 = table.Column<int>(type: "NUMERIC", nullable: true),
                    similarity3 = table.Column<double>(type: "NUMERIC", nullable: true),
                    recommendation4 = table.Column<int>(type: "NUMERIC", nullable: true),
                    similarity4 = table.Column<double>(type: "NUMERIC", nullable: true),
                    recommendation5 = table.Column<int>(type: "NUMERIC", nullable: true),
                    similarity5 = table.Column<double>(type: "NUMERIC", nullable: true),
                    popularity_rank = table.Column<double>(type: "NUMERIC", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.product_ID);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rec_prod_id_1 = table.Column<int>(type: "INTEGER", nullable: false),
                    rec_prod_id_2 = table.Column<int>(type: "INTEGER", nullable: false),
                    rec_prod_id_3 = table.Column<int>(type: "INTEGER", nullable: false),
                    rec_prod_id_4 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.customer_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_customer_ID",
                table: "Customers",
                column: "customer_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_transaction_ID",
                table: "Orders",
                column: "transaction_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_product_ID",
                table: "Products",
                column: "product_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "LineItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
