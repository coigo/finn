using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "CriadoEm",
                value: new DateTime(2025, 2, 9, 11, 44, 56, 433, DateTimeKind.Local).AddTicks(2975));

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "CriadoEm",
                value: new DateTime(2025, 2, 9, 11, 44, 56, 433, DateTimeKind.Local).AddTicks(3011));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "CriadoEm",
                value: new DateTime(2025, 2, 8, 14, 40, 5, 501, DateTimeKind.Local).AddTicks(2428));

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "CriadoEm",
                value: new DateTime(2025, 2, 8, 14, 40, 5, 501, DateTimeKind.Local).AddTicks(2462));

            migrationBuilder.InsertData(
                table: "MovimentacoesCategorias",
                columns: new[] { "Id", "CriadoEm", "DeletadoEm", "Nome", "Tipo" },
                values: new object[] { 3, new DateTime(2025, 2, 8, 14, 40, 5, 501, DateTimeKind.Local).AddTicks(2464), null, "Investimentos", "SAIDA" });
        }
    }
}
