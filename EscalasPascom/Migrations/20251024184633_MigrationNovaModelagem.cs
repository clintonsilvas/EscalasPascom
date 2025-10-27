using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EscalasPascom.Migrations
{
    /// <inheritdoc />
    public partial class MigrationNovaModelagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Indisponibilidades_Pessoas_PessoaId",
                table: "Indisponibilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Escalas_EscalaId",
                table: "Missas");

            migrationBuilder.DropTable(
                name: "MissaPessoaCameraSom");

            migrationBuilder.DropTable(
                name: "PapelPessoa");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Missas_EscalaId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "EscalaId",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "TemMidia",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "TemTransmissao",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "Mes",
                table: "Escalas");

            migrationBuilder.RenameColumn(
                name: "QuantidadeCameraSom",
                table: "Missas",
                newName: "ParoquiaId");

            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "Escalas",
                newName: "PastoralId");

            migrationBuilder.AddColumn<int>(
                name: "IdPastoral",
                table: "Papeis",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PastoralId",
                table: "Papeis",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Papeis",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdParoquia",
                table: "Missas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PessoaId",
                table: "Indisponibilidades",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Escalas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdPastoral",
                table: "Escalas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    NomeCompleto = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoUsuario = table.Column<string>(type: "text", nullable: false),
                    IsAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EscalaMissa",
                columns: table => new
                {
                    EscalasId = table.Column<int>(type: "integer", nullable: false),
                    MissasId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalaMissa", x => new { x.EscalasId, x.MissasId });
                    table.ForeignKey(
                        name: "FK_EscalaMissa_Escalas_EscalasId",
                        column: x => x.EscalasId,
                        principalTable: "Escalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EscalaMissa_Missas_MissasId",
                        column: x => x.MissasId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MissaEscalas",
                columns: table => new
                {
                    IdMissa = table.Column<int>(type: "integer", nullable: false),
                    IdEscala = table.Column<int>(type: "integer", nullable: false),
                    MissaId = table.Column<int>(type: "integer", nullable: false),
                    EscalaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissaEscalas", x => new { x.IdMissa, x.IdEscala });
                    table.ForeignKey(
                        name: "FK_MissaEscalas_Escalas_EscalaId",
                        column: x => x.EscalaId,
                        principalTable: "Escalas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissaEscalas_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MissaPapeis",
                columns: table => new
                {
                    IdPapel = table.Column<int>(type: "integer", nullable: false),
                    IdMissa = table.Column<int>(type: "integer", nullable: false),
                    PapelId = table.Column<int>(type: "integer", nullable: false),
                    MissaId = table.Column<int>(type: "integer", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissaPapeis", x => new { x.IdMissa, x.IdPapel });
                    table.ForeignKey(
                        name: "FK_MissaPapeis_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissaPapeis_Papeis_PapelId",
                        column: x => x.PapelId,
                        principalTable: "Papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paroquias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    Diocese = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paroquias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioMissas",
                columns: table => new
                {
                    IdMissa = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IdPapel = table.Column<int>(type: "integer", nullable: false),
                    MissaId = table.Column<int>(type: "integer", nullable: false),
                    PapelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioMissas", x => new { x.IdUsuario, x.IdMissa, x.IdPapel });
                    table.ForeignKey(
                        name: "FK_UsuarioMissas_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioMissas_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioMissas_Papeis_PapelId",
                        column: x => x.PapelId,
                        principalTable: "Papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pastorais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    IdParoquia = table.Column<int>(type: "integer", nullable: false),
                    ParoquiaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pastorais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pastorais_Paroquias_ParoquiaId",
                        column: x => x.ParoquiaId,
                        principalTable: "Paroquias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PastoralUsuario",
                columns: table => new
                {
                    PastoraisId = table.Column<int>(type: "integer", nullable: false),
                    UsuariosId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastoralUsuario", x => new { x.PastoraisId, x.UsuariosId });
                    table.ForeignKey(
                        name: "FK_PastoralUsuario_AspNetUsers_UsuariosId",
                        column: x => x.UsuariosId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PastoralUsuario_Pastorais_PastoraisId",
                        column: x => x.PastoraisId,
                        principalTable: "Pastorais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPapelPastoral",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IdPapel = table.Column<int>(type: "integer", nullable: false),
                    IdPastoral = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<string>(type: "text", nullable: false),
                    PapelId = table.Column<int>(type: "integer", nullable: false),
                    PastoralId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPapelPastoral", x => new { x.IdUsuario, x.IdPapel, x.IdPastoral });
                    table.ForeignKey(
                        name: "FK_UsuarioPapelPastoral_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPapelPastoral_Papeis_PapelId",
                        column: x => x.PapelId,
                        principalTable: "Papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPapelPastoral_Pastorais_PastoralId",
                        column: x => x.PastoralId,
                        principalTable: "Pastorais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Papeis_Descricao_IdPastoral",
                table: "Papeis",
                columns: new[] { "Descricao", "IdPastoral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Papeis_PastoralId",
                table: "Papeis",
                column: "PastoralId");

            migrationBuilder.CreateIndex(
                name: "IX_Papeis_UsuarioId",
                table: "Papeis",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Missas_ParoquiaId",
                table: "Missas",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Escalas_PastoralId",
                table: "Escalas",
                column: "PastoralId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EscalaMissa_MissasId",
                table: "EscalaMissa",
                column: "MissasId");

            migrationBuilder.CreateIndex(
                name: "IX_MissaEscalas_EscalaId",
                table: "MissaEscalas",
                column: "EscalaId");

            migrationBuilder.CreateIndex(
                name: "IX_MissaEscalas_MissaId",
                table: "MissaEscalas",
                column: "MissaId");

            migrationBuilder.CreateIndex(
                name: "IX_MissaPapeis_MissaId",
                table: "MissaPapeis",
                column: "MissaId");

            migrationBuilder.CreateIndex(
                name: "IX_MissaPapeis_PapelId",
                table: "MissaPapeis",
                column: "PapelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pastorais_ParoquiaId",
                table: "Pastorais",
                column: "ParoquiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PastoralUsuario_UsuariosId",
                table: "PastoralUsuario",
                column: "UsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioMissas_MissaId",
                table: "UsuarioMissas",
                column: "MissaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioMissas_PapelId",
                table: "UsuarioMissas",
                column: "PapelId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPapelPastoral_PapelId",
                table: "UsuarioPapelPastoral",
                column: "PapelId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPapelPastoral_PastoralId",
                table: "UsuarioPapelPastoral",
                column: "PastoralId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPapelPastoral_UsuarioId",
                table: "UsuarioPapelPastoral",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Escalas_Pastorais_PastoralId",
                table: "Escalas",
                column: "PastoralId",
                principalTable: "Pastorais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Indisponibilidades_AspNetUsers_PessoaId",
                table: "Indisponibilidades",
                column: "PessoaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Paroquias_ParoquiaId",
                table: "Missas",
                column: "ParoquiaId",
                principalTable: "Paroquias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Papeis_AspNetUsers_UsuarioId",
                table: "Papeis",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Papeis_Pastorais_PastoralId",
                table: "Papeis",
                column: "PastoralId",
                principalTable: "Pastorais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Escalas_Pastorais_PastoralId",
                table: "Escalas");

            migrationBuilder.DropForeignKey(
                name: "FK_Indisponibilidades_AspNetUsers_PessoaId",
                table: "Indisponibilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Missas_Paroquias_ParoquiaId",
                table: "Missas");

            migrationBuilder.DropForeignKey(
                name: "FK_Papeis_AspNetUsers_UsuarioId",
                table: "Papeis");

            migrationBuilder.DropForeignKey(
                name: "FK_Papeis_Pastorais_PastoralId",
                table: "Papeis");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EscalaMissa");

            migrationBuilder.DropTable(
                name: "MissaEscalas");

            migrationBuilder.DropTable(
                name: "MissaPapeis");

            migrationBuilder.DropTable(
                name: "PastoralUsuario");

            migrationBuilder.DropTable(
                name: "UsuarioMissas");

            migrationBuilder.DropTable(
                name: "UsuarioPapelPastoral");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pastorais");

            migrationBuilder.DropTable(
                name: "Paroquias");

            migrationBuilder.DropIndex(
                name: "IX_Papeis_Descricao_IdPastoral",
                table: "Papeis");

            migrationBuilder.DropIndex(
                name: "IX_Papeis_PastoralId",
                table: "Papeis");

            migrationBuilder.DropIndex(
                name: "IX_Papeis_UsuarioId",
                table: "Papeis");

            migrationBuilder.DropIndex(
                name: "IX_Missas_ParoquiaId",
                table: "Missas");

            migrationBuilder.DropIndex(
                name: "IX_Escalas_PastoralId",
                table: "Escalas");

            migrationBuilder.DropColumn(
                name: "IdPastoral",
                table: "Papeis");

            migrationBuilder.DropColumn(
                name: "PastoralId",
                table: "Papeis");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Papeis");

            migrationBuilder.DropColumn(
                name: "IdParoquia",
                table: "Missas");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Escalas");

            migrationBuilder.DropColumn(
                name: "IdPastoral",
                table: "Escalas");

            migrationBuilder.RenameColumn(
                name: "ParoquiaId",
                table: "Missas",
                newName: "QuantidadeCameraSom");

            migrationBuilder.RenameColumn(
                name: "PastoralId",
                table: "Escalas",
                newName: "Ano");

            migrationBuilder.AddColumn<int>(
                name: "EscalaId",
                table: "Missas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TemMidia",
                table: "Missas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TemTransmissao",
                table: "Missas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "PessoaId",
                table: "Indisponibilidades",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Mes",
                table: "Escalas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissaPessoaCameraSom",
                columns: table => new
                {
                    MissaId = table.Column<int>(type: "integer", nullable: false),
                    PessoaId = table.Column<int>(type: "integer", nullable: false),
                    PapelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissaPessoaCameraSom", x => new { x.MissaId, x.PessoaId, x.PapelId });
                    table.ForeignKey(
                        name: "FK_MissaPessoaCameraSom_Missas_MissaId",
                        column: x => x.MissaId,
                        principalTable: "Missas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissaPessoaCameraSom_Papeis_PapelId",
                        column: x => x.PapelId,
                        principalTable: "Papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissaPessoaCameraSom_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PapelPessoa",
                columns: table => new
                {
                    PapeisId = table.Column<int>(type: "integer", nullable: false),
                    PessoasId = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Missas_EscalaId",
                table: "Missas",
                column: "EscalaId");

            migrationBuilder.CreateIndex(
                name: "IX_MissaPessoaCameraSom_PapelId",
                table: "MissaPessoaCameraSom",
                column: "PapelId");

            migrationBuilder.CreateIndex(
                name: "IX_MissaPessoaCameraSom_PessoaId",
                table: "MissaPessoaCameraSom",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_PapelPessoa_PessoasId",
                table: "PapelPessoa",
                column: "PessoasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indisponibilidades_Pessoas_PessoaId",
                table: "Indisponibilidades",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Missas_Escalas_EscalaId",
                table: "Missas",
                column: "EscalaId",
                principalTable: "Escalas",
                principalColumn: "Id");
        }
    }
}
