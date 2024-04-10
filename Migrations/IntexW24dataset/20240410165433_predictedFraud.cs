using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INTEXII.Migrations.IntexW24dataset
{
    /// <inheritdoc />
    public partial class predictedFraud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "fraud",
                table: "Orders",
                type: "NUMERIC",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "pred_fraud",
                table: "Orders",
                type: "NUMERIC",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pred_fraud",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "fraud",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMERIC");
        }
    }
}
