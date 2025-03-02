using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Aportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_movimentacoes_categorias_categoria_id",
                table: "movimentacoes");

            migrationBuilder.RenameColumn(
                name: "categoria_id",
                table: "movimentacoes",
                newName: "categoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_movimentacoes_categoria_id",
                table: "movimentacoes",
                newName: "IX_movimentacoes_categoriaId");

            migrationBuilder.CreateTable(
                name: "aportes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    precoMedio = table.Column<float>(type: "REAL", nullable: false),
                    identificador = table.Column<string>(type: "TEXT", nullable: false),
                    categoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Categoria = table.Column<int>(type: "INTEGER", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aportes", x => x.id);
                    table.UniqueConstraint("AK_aportes_identificador", x => x.identificador);
                });

            migrationBuilder.CreateTable(
                name: "aportes_historico",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    precoMedio = table.Column<float>(type: "REAL", nullable: false),
                    identificador = table.Column<string>(type: "TEXT", nullable: false),
                    tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    criadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aportes_historico", x => x.id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_aportes_historico_identificador",
                table: "aportes_historico",
                column: "identificador");

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_movimentacoes_categorias_categoriaId",
                table: "movimentacoes",
                column: "categoriaId",
                principalTable: "movimentacoes_categorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_movimentacoes_categorias_categoriaId",
                table: "movimentacoes");

            migrationBuilder.DropTable(
                name: "aportes");

            migrationBuilder.DropTable(
                name: "aportes_historico");

            migrationBuilder.RenameColumn(
                name: "categoriaId",
                table: "movimentacoes",
                newName: "categoria_id");

            migrationBuilder.RenameIndex(
                name: "IX_movimentacoes_categoriaId",
                table: "movimentacoes",
                newName: "IX_movimentacoes_categoria_id");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5343));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5381));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5384));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5385));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5387));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5389));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5390));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5393));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5394));

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_movimentacoes_categorias_categoria_id",
                table: "movimentacoes",
                column: "categoria_id",
                principalTable: "movimentacoes_categorias",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
