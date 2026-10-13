using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoGestorApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDevOps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UNIDADE",
                columns: table => new
                {
                    ID_UNIDADE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME_UNIDADE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CIDADE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RESPONSAVEL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    META_RECICLAGEM_KG = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIDADE", x => x.ID_UNIDADE);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SENHA = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PERFIL = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "PONTO_COLETA",
                columns: table => new
                {
                    ID_PONTO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_UNIDADE = table.Column<int>(type: "int", nullable: false),
                    TIPO_RESIDUO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CAPACIDADE_MAX_KG = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NIVEL_ATUAL_KG = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    STATUS_PONTO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PONTO_COLETA", x => x.ID_PONTO);
                    table.ForeignKey(
                        name: "FK_PONTO_COLETA_UNIDADE_ID_UNIDADE",
                        column: x => x.ID_UNIDADE,
                        principalTable: "UNIDADE",
                        principalColumn: "ID_UNIDADE",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ALERTA_COLETA",
                columns: table => new
                {
                    ID_ALERTA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PONTO = table.Column<int>(type: "int", nullable: false),
                    DATA_ALERTA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TIPO_ALERTA = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MENSAGEM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTA_COLETA", x => x.ID_ALERTA);
                    table.ForeignKey(
                        name: "FK_ALERTA_COLETA_PONTO_COLETA_ID_PONTO",
                        column: x => x.ID_PONTO,
                        principalTable: "PONTO_COLETA",
                        principalColumn: "ID_PONTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COLETA_RESIDUO",
                columns: table => new
                {
                    ID_COLETA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PONTO = table.Column<int>(type: "int", nullable: false),
                    DATA_COLETA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QUANTIDADE_KG = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DESTINO_PREVISTO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    STATUS_COLETA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLETA_RESIDUO", x => x.ID_COLETA);
                    table.ForeignKey(
                        name: "FK_COLETA_RESIDUO_PONTO_COLETA_ID_PONTO",
                        column: x => x.ID_PONTO,
                        principalTable: "PONTO_COLETA",
                        principalColumn: "ID_PONTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DESTINACAO_RESIDUO",
                columns: table => new
                {
                    ID_DESTINACAO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_COLETA = table.Column<int>(type: "int", nullable: false),
                    DATA_DESTINACAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMPRESA_DESTINO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FORMA_TRATAMENTO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DESTINACAO_RESIDUO", x => x.ID_DESTINACAO);
                    table.ForeignKey(
                        name: "FK_DESTINACAO_RESIDUO_COLETA_RESIDUO_ID_COLETA",
                        column: x => x.ID_COLETA,
                        principalTable: "COLETA_RESIDUO",
                        principalColumn: "ID_COLETA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "UNIDADE",
                columns: new[] { "ID_UNIDADE", "CIDADE", "META_RECICLAGEM_KG", "NOME_UNIDADE", "RESPONSAVEL" },
                values: new object[,]
                {
                    { 1, "São Paulo", 500m, "Unidade Centro", "Giovanni Luiz" },
                    { 2, "Jundiaí", 350m, "Unidade Jundiaí", "Equipe ESG" }
                });

            migrationBuilder.InsertData(
                table: "PONTO_COLETA",
                columns: new[] { "ID_PONTO", "CAPACIDADE_MAX_KG", "ID_UNIDADE", "NIVEL_ATUAL_KG", "STATUS_PONTO", "TIPO_RESIDUO" },
                values: new object[,]
                {
                    { 1, 100m, 1, 75m, "ATENCAO", "PLASTICO" },
                    { 2, 80m, 2, 68m, "CRITICO", "PAPEL" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERTA_COLETA_ID_PONTO",
                table: "ALERTA_COLETA",
                column: "ID_PONTO");

            migrationBuilder.CreateIndex(
                name: "IX_COLETA_RESIDUO_ID_PONTO",
                table: "COLETA_RESIDUO",
                column: "ID_PONTO");

            migrationBuilder.CreateIndex(
                name: "IX_DESTINACAO_RESIDUO_ID_COLETA",
                table: "DESTINACAO_RESIDUO",
                column: "ID_COLETA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PONTO_COLETA_ID_UNIDADE",
                table: "PONTO_COLETA",
                column: "ID_UNIDADE");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_EMAIL",
                table: "USUARIO",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERTA_COLETA");

            migrationBuilder.DropTable(
                name: "DESTINACAO_RESIDUO");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "COLETA_RESIDUO");

            migrationBuilder.DropTable(
                name: "PONTO_COLETA");

            migrationBuilder.DropTable(
                name: "UNIDADE");
        }
    }
}
