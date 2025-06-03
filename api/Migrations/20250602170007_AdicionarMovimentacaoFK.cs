using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMovimentacaoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "parcelaId",
                table: "movimentacoes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "persistenteId",
                table: "movimentacoes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4760));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4783));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4784));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4785));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4786));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4787));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4788));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4789));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4789));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4790));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 13, 0, 7, 636, DateTimeKind.Local).AddTicks(4792));

            migrationBuilder.CreateIndex(
                name: "IX_movimentacoes_parcelaId",
                table: "movimentacoes",
                column: "parcelaId");

            migrationBuilder.CreateIndex(
                name: "IX_movimentacoes_persistenteId",
                table: "movimentacoes",
                column: "persistenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_movimentacoes_parcelas_parcelaId",
                table: "movimentacoes",
                column: "parcelaId",
                principalTable: "movimentacoes_parcelas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_movimentacoes_persistentes_persistenteId",
                table: "movimentacoes",
                column: "persistenteId",
                principalTable: "movimentacoes_persistentes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_movimentacoes_parcelas_parcelaId",
                table: "movimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_movimentacoes_persistentes_persistenteId",
                table: "movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_movimentacoes_parcelaId",
                table: "movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_movimentacoes_persistenteId",
                table: "movimentacoes");

            migrationBuilder.DropColumn(
                name: "parcelaId",
                table: "movimentacoes");

            migrationBuilder.DropColumn(
                name: "persistenteId",
                table: "movimentacoes");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(4994));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5016));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5018));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5019));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 6, 2, 12, 26, 35, 413, DateTimeKind.Local).AddTicks(5019));
        }
    }
}
