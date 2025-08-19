using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class mudanca1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Papeis_Pessoas_PessoaId",
                table: "Papeis");

            migrationBuilder.DropIndex(
                name: "IX_Papeis_PessoaId",
                table: "Papeis");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "Papeis");

            migrationBuilder.CreateTable(
                name: "PapelPessoa",
                columns: table => new
                {
                    PapeisId = table.Column<int>(type: "int", nullable: false),
                    PessoasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PapelPessoa", x => new { x.PapeisId, x.PessoasId });
                    table.ForeignKey(
                        name: "FK_PapelPessoa_Papeis_PapeisId",
                        column: x => x.PapeisId,
                        principalTable: "Papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PapelPessoa_Pessoas_PessoasId",
                        column: x => x.PessoasId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PapelPessoa_PessoasId",
                table: "PapelPessoa",
                column: "PessoasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PapelPessoa");

            migrationBuilder.AddColumn<int>(
                name: "PessoaId",
                table: "Papeis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Papeis_PessoaId",
                table: "Papeis",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Papeis_Pessoas_PessoaId",
                table: "Papeis",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id");
        }
    }
}
