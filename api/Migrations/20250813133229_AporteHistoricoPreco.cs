using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AporteHistoricoPreco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "precoMedio",
                table: "aportes_historico",
                newName: "preco");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(387));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(410));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(414));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(486));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(487));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(488));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(489));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(489));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(490));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 8, 13, 9, 32, 29, 272, DateTimeKind.Local).AddTicks(491));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "preco",
                table: "aportes_historico",
                newName: "precoMedio");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4760));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4783));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4784));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4785));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4786));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4787));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4788));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4789));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4789));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4790));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4792));
        }
    }
}
