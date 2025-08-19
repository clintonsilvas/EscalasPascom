using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class criacaoEscala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EscalaId",
                table: "Missas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Escalas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escalas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Missas_EscalaId",
                table: "Missas",
                column: "EscalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Escalas_EscalaId",
                table: "Missas",
                column: "EscalaId",
                principalTable: "Escalas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Escalas_EscalaId",
                table: "Missas");

            migrationBuilder.DropTable(
                name: "Escalas");

            migrationBuilder.DropIndex(
                name: "IX_Missas_EscalaId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "EscalaId",
                table: "Missas");
        }
    }
}
