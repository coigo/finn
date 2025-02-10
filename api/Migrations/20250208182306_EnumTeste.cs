using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class EnumTeste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "MovimentacoesCategorias",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CriadoEm", "Tipo" },
                values: new object[] { new DateTime(2025, 2, 8, 14, 23, 6, 473, DateTimeKind.Local).AddTicks(3624), "SAIDA" });

            migrationBuilder.AddCheckConstraint(
                name: "Movimentacao_Categoria_Tipo",
                table: "MovimentacoesCategorias",
                sql: "[Tipo] in ('ENTRADA', 'SAIDA')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Movimentacao_Categoria_Tipo",
                table: "MovimentacoesCategorias");

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "MovimentacoesCategorias",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "MovimentacoesCategorias",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CriadoEm", "Tipo" },
                values: new object[] { new DateTime(2025, 2, 8, 11, 45, 28, 874, DateTimeKind.Local).AddTicks(3469), 1 });
        }
    }
}
