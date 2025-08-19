using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class Pessoa_TemFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Pessoas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Pessoas");
        }
    }
}
