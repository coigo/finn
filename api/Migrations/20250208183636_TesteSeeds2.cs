using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TesteSeeds2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Movimentacao_Categoria_Tipo",
                table: "MovimentacoesCategorias");

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "CriadoEm",
                value: new DateTime(2025, 2, 8, 14, 36, 36, 27, DateTimeKind.Local).AddTicks(7493));

            migrationBuilder.InsertData(
                table: "MovimentacoesCategorias",
                columns: new[] { "Id", "CriadoEm", "DeletadoEm", "Nome", "Tipo" },
                values: new object[] { 2, new DateTime(2025, 2, 8, 14, 36, 36, 27, DateTimeKind.Local).AddTicks(7542), null, "Educação", "SAIDA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "CriadoEm",
                value: new DateTime(2025, 2, 8, 14, 24, 48, 757, DateTimeKind.Local).AddTicks(9114));

            migrationBuilder.AddCheckConstraint(
                name: "Movimentacao_Categoria_Tipo",
                table: "MovimentacoesCategorias",
                sql: "[Tipo] in ('ENTRADA', 'SAIDA')");
        }
    }
}
