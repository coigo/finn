using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AporteData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data",
                table: "aportes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8193));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8234));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8236));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8237));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8239));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8240));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8241));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8242));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8244));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8245));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8246));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8247));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 3, 5, 10, 51, 31, 661, DateTimeKind.Local).AddTicks(8248));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "aportes");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7796));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7839));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7843));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7845));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7846));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7847));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7848));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7849));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7851));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7852));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7853));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 3, 3, 10, 9, 0, 283, DateTimeKind.Local).AddTicks(7854));
        }
    }
}
