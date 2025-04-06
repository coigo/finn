using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AporteDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "resumo_historico",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "resumo",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "movimentacoes_parcelas",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "movimentacoes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "precoMedio",
                table: "aportes_historico",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantidade",
                table: "aportes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "precoMedio",
                table: "aportes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4937));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4977));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4979));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4980));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4982));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4983));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4984));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4985));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4987));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4988));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4989));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 4, 6, 15, 55, 37, 361, DateTimeKind.Local).AddTicks(4991));

            migrationBuilder.UpdateData(
                table: "resumo",
                keyColumn: "id",
                keyValue: 1,
                column: "valor",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "resumo",
                keyColumn: "id",
                keyValue: 2,
                column: "valor",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "valor",
                table: "resumo_historico",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "valor",
                table: "resumo",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "valor",
                table: "movimentacoes_parcelas",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "valor",
                table: "movimentacoes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "precoMedio",
                table: "aportes_historico",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "quantidade",
                table: "aportes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "precoMedio",
                table: "aportes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7085));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7134));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7140));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 12, 0, 8, 174, DateTimeKind.Local).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "resumo",
                keyColumn: "id",
                keyValue: 1,
                column: "valor",
                value: 0f);

            migrationBuilder.UpdateData(
                table: "resumo",
                keyColumn: "id",
                keyValue: 2,
                column: "valor",
                value: 0f);
        }
    }
}
