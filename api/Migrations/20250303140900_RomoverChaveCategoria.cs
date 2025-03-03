using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RomoverChaveCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoriaId",
                table: "aportes");

            migrationBuilder.RenameColumn(
                name: "Categoria",
                table: "aportes",
                newName: "categoria");

            migrationBuilder.AddColumn<int>(
                name: "categoria",
                table: "aportes_historico",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "quantidade",
                table: "aportes",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categoria",
                table: "aportes_historico");

            migrationBuilder.DropColumn(
                name: "quantidade",
                table: "aportes");

            migrationBuilder.RenameColumn(
                name: "categoria",
                table: "aportes",
                newName: "Categoria");

            migrationBuilder.AddColumn<int>(
                name: "categoriaId",
                table: "aportes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4039));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4077));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4079));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4080));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4081));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4082));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4083));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4085));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4086));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4087));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4088));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4089));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 3, 1, 13, 33, 7, 592, DateTimeKind.Local).AddTicks(4090));
        }
    }
}
