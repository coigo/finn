using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenomearTabelaResumo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "resumo",
                newName: "saldos"
            );

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8302));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8389));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8391));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8393));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8394));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8395));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8396));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8397));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8399));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8400));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8401));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8402));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 27, 58, 674, DateTimeKind.Local).AddTicks(8403));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2421));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2464));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2466));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2467));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2470));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2471));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2472));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2475));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2477));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 8, 22, 15, 58, 5, 223, DateTimeKind.Local).AddTicks(2478));
        }
    }
}
