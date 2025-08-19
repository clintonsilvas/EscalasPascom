using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class acrecentarPapelRelacionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MissaPessoaCameraSom",
                table: "MissaPessoaCameraSom");

            migrationBuilder.AddColumn<int>(
                name: "PapelId",
                table: "MissaPessoaCameraSom",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MissaPessoaCameraSom",
                table: "MissaPessoaCameraSom",
                columns: new[] { "MissaId", "PessoaId", "PapelId" });

            migrationBuilder.CreateIndex(
                name: "IX_MissaPessoaCameraSom_PapelId",
                table: "MissaPessoaCameraSom",
                column: "PapelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissaPessoaCameraSom_Papeis_PapelId",
                table: "MissaPessoaCameraSom",
                column: "PapelId",
                principalTable: "Papeis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissaPessoaCameraSom_Papeis_PapelId",
                table: "MissaPessoaCameraSom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MissaPessoaCameraSom",
                table: "MissaPessoaCameraSom");

            migrationBuilder.DropIndex(
                name: "IX_MissaPessoaCameraSom_PapelId",
                table: "MissaPessoaCameraSom");

            migrationBuilder.DropColumn(
                name: "PapelId",
                table: "MissaPessoaCameraSom");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MissaPessoaCameraSom",
                table: "MissaPessoaCameraSom",
                columns: new[] { "MissaId", "PessoaId" });
        }
    }
}
