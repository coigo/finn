using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class VencimentoParcelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "vencimento",
                table: "movimentacoes_parcelas",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 2, 14, 16, 38, 40, 52, DateTimeKind.Local).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 2, 14, 16, 38, 40, 52, DateTimeKind.Local).AddTicks(9507));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vencimento",
                table: "movimentacoes_parcelas");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 2, 10, 18, 56, 48, 861, DateTimeKind.Local).AddTicks(8553));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 2, 10, 18, 56, 48, 861, DateTimeKind.Local).AddTicks(8599));
        }
    }
}
