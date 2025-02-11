using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenomearColunas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movimentacoes_categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    tipo = table.Column<string>(type: "TEXT", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    deletadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimentacoes_categorias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movimentacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    valor = table.Column<float>(type: "REAL", nullable: false),
                    tipo = table.Column<string>(type: "TEXT", nullable: false),
                    categoria_id = table.Column<int>(type: "INTEGER", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimentacoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_movimentacoes_movimentacoes_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "movimentacoes_categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movimentacoes_parcelas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    movimentacao_id = table.Column<int>(type: "INTEGER", nullable: false),
                    numero = table.Column<int>(type: "INTEGER", nullable: false),
                    valor = table.Column<float>(type: "REAL", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimentacoes_parcelas", x => x.id);
                    table.ForeignKey(
                        name: "FK_movimentacoes_parcelas_movimentacoes_movimentacao_id",
                        column: x => x.movimentacao_id,
                        principalTable: "movimentacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "movimentacoes_categorias",
                columns: new[] { "id", "criadoEm", "deletadoEm", "nome", "tipo" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 10, 18, 56, 48, 861, DateTimeKind.Local).AddTicks(8553), null, "Comida", "SAIDA" },
                    { 2, new DateTime(2025, 2, 10, 18, 56, 48, 861, DateTimeKind.Local).AddTicks(8599), null, "Educação", "SAIDA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_movimentacoes_categoria_id",
                table: "movimentacoes",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacoes_parcelas_movimentacao_id",
                table: "movimentacoes_parcelas",
                column: "movimentacao_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movimentacoes_parcelas");

            migrationBuilder.DropTable(
                name: "movimentacoes");

            migrationBuilder.DropTable(
                name: "movimentacoes_categorias");
        }
    }
}
