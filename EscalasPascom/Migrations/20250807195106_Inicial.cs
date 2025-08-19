using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indisponibilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PessoaId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indisponibilidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Missas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperadorId = table.Column<int>(type: "int", nullable: true),
                    MidiaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoas_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Papeis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PessoaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papeis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Papeis_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Indisponibilidades_PessoaId",
                table: "Indisponibilidades",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_MidiaId",
                table: "Missas",
                column: "MidiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_OperadorId",
                table: "Missas",
                column: "OperadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Papeis_PessoaId",
                table: "Papeis",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_MissaId",
                table: "Pessoas",
                column: "MissaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indisponibilidades_Pessoas_PessoaId",
                table: "Indisponibilidades",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Pessoas_MidiaId",
                table: "Missas");

            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Pessoas_OperadorId",
                table: "Missas");

            migrationBuilder.DropTable(
                name: "Indisponibilidades");

            migrationBuilder.DropTable(
                name: "Papeis");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "Missas");
        }
    }
}
