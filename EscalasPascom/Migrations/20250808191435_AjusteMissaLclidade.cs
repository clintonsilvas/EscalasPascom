using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class AjusteMissaLclidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Local",
                table: "Missas");

            migrationBuilder.AddColumn<int>(
                name: "LocalId",
                table: "Missas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Missas_LocalId",
                table: "Missas",
                column: "LocalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Locais_LocalId",
                table: "Missas",
                column: "LocalId",
                principalTable: "Locais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Locais_LocalId",
                table: "Missas");

            migrationBuilder.DropIndex(
                name: "IX_Missas_LocalId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "Missas");

            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "Missas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
