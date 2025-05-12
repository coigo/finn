using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Salario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salarios", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5865));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5888));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5889));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5892));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5892));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5893));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5894));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5895));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 5, 12, 17, 28, 33, 455, DateTimeKind.Local).AddTicks(5895));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salarios");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4550));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4552));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4555));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4556));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4557));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4558));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4559));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4561));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4562));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 4, 21, 9, 11, 49, 0, DateTimeKind.Local).AddTicks(4563));
        }
    }
}
