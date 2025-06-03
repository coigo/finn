using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarParcelaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categoriaId",
                table: "movimentacoes_parcelas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tipo",
                table: "movimentacoes_parcelas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(4994));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5016));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5018));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5019));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5019));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoriaId",
                table: "movimentacoes_parcelas");

            migrationBuilder.DropColumn(
                name: "tipo",
                table: "movimentacoes_parcelas");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6599));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6653));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6654));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6655));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6656));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6658));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6659));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6660));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6661));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6663));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6664));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6665));
        }
    }
}
