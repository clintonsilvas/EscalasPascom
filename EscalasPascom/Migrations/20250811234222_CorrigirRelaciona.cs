using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirRelaciona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Pessoas_MidiaId",
                table: "Missas");

            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Pessoas_OperadorId",
                table: "Missas");

            migrationBuilder.DropForeignKey(
                name: "FK_Pessoas_Missas_MissaId",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_MissaId",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Missas_MidiaId",
                table: "Missas");

            migrationBuilder.DropIndex(
                name: "IX_Missas_OperadorId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "MissaId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "MidiaId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "OperadorId",
                table: "Missas");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeCameraSom",
                table: "Missas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MissaPessoaCameraSom",
                columns: table => new
                {
                    MissaId = table.Column<int>(type: "int", nullable: false),
                    PessoaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissaPessoaCameraSom", x => new { x.MissaId, x.PessoaId });
                    table.ForeignKey(
                        name: "FK_MissaPessoaCameraSom_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissaPessoaCameraSom_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissaPessoaCameraSom_PessoaId",
                table: "MissaPessoaCameraSom",
                column: "PessoaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissaPessoaCameraSom");

            migrationBuilder.DropColumn(
                name: "QuantidadeCameraSom",
                table: "Missas");

            migrationBuilder.AddColumn<int>(
                name: "MissaId",
                table: "Pessoas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MidiaId",
                table: "Missas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperadorId",
                table: "Missas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_MissaId",
                table: "Pessoas",
                column: "MissaId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_MidiaId",
                table: "Missas",
                column: "MidiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_OperadorId",
                table: "Missas",
                column: "OperadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Pessoas_MidiaId",
                table: "Missas",
                column: "MidiaId",
                principalTable: "Pessoas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Pessoas_OperadorId",
                table: "Missas",
                column: "OperadorId",
                principalTable: "Pessoas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoas_Missas_MissaId",
                table: "Pessoas",
                column: "MissaId",
                principalTable: "Missas",
                principalColumn: "Id");
        }
    }
}
