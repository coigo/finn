using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MovimentacaoPersistenteDeletadoEm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deletadoEm",
                table: "movimentacoes_persistentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5857));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5858));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5860));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5861));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5862));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5863));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5864));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5866));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5867));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5868));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5869));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 11, 29, 10, 1, 35, 968, DateTimeKind.Local).AddTicks(5870));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deletadoEm",
                table: "movimentacoes_persistentes");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7204));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7256));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7257));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7258));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7261));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7262));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7263));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7264));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7265));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7266));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 11, 22, 9, 55, 12, 238, DateTimeKind.Local).AddTicks(7268));
        }
    }
}
