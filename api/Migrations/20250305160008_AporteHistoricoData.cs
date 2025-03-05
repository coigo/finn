using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AporteHistoricoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "aportes");

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "aportes_historico",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "categoria",
                table: "aportes_historico",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "data",
                table: "aportes_historico",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "categoria",
                table: "aportes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "aportes_historico");

            migrationBuilder.AlterColumn<int>(
                name: "tipo",
                table: "aportes_historico",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "categoria",
                table: "aportes_historico",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "categoria",
                table: "aportes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

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
    }
}
