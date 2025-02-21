using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Resumo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "resumo",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    valor = table.Column<float>(type: "REAL", nullable: false),
                    nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resumo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "resumo_historico",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    valor = table.Column<float>(type: "REAL", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    resumoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resumo_historico", x => x.id);
                    table.ForeignKey(
                        name: "FK_resumo_historico_resumo_resumoId",
                        column: x => x.resumoId,
                        principalTable: "resumo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "criadoEm", "nome" },
                values: new object[] { new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5343), "Comida e Mercado" });

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "criadoEm", "nome" },
                values: new object[] { new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5380), "Educação e Desenvolvimento" });

            migrationBuilder.InsertData(
                table: "movimentacoes_categorias",
                columns: new[] { "id", "criadoEm", "deletadoEm", "nome", "tipo" },
                values: new object[,]
                {
                    { 3, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5381), null, "Investimentos", "SAIDA" },
                    { 4, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5383), null, "Lazer e Bem-estar", "SAIDA" },
                    { 5, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5384), null, "Serviços", "SAIDA" },
                    { 6, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5385), null, "Moradia", "SAIDA" },
                    { 7, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5387), null, "Transporte", "SAIDA" },
                    { 8, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5388), null, "Saúde", "SAIDA" },
                    { 9, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5389), null, "Salário", "ENTRADA" },
                    { 10, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5390), null, "Dividendo", "ENTRADA" },
                    { 11, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5392), null, "Venda", "ENTRADA" },
                    { 12, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5393), null, "Transferências", "ENTRADA" },
                    { 13, new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5394), null, "Outros", "ENTRADA" }
                });

            migrationBuilder.InsertData(
                table: "resumo",
                columns: new[] { "id", "nome", "valor" },
                values: new object[,]
                {
                    { 1, "Corrente", 0f },
                    { 2, "Investimentos", 0f }
                });

            migrationBuilder.CreateIndex(
                name: "IX_resumo_historico_resumoId",
                table: "resumo_historico",
                column: "resumoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resumo_historico");

            migrationBuilder.DropTable(
                name: "resumo");

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "criadoEm", "nome" },
                values: new object[] { new DateTime(2025, 2, 14, 16, 38, 40, 52, DateTimeKind.Local).AddTicks(9472), "Comida" });

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "criadoEm", "nome" },
                values: new object[] { new DateTime(2025, 2, 14, 16, 38, 40, 52, DateTimeKind.Local).AddTicks(9507), "Educação" });
        }
    }
}
