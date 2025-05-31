using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoverMovimentacaoidParcelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_parcelas_movimentacoes_MovimentacaoId",
                table: "movimentacoes_parcelas");

            migrationBuilder.DropIndex(
                name: "IX_movimentacoes_parcelas_MovimentacaoId",
                table: "movimentacoes_parcelas");

            migrationBuilder.DropColumn(
                name: "MovimentacaoId",
                table: "movimentacoes_parcelas");

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1308));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1366));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1372));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1373));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1374));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1375));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1377));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1378));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1379));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 14, 32, 40, 580, DateTimeKind.Local).AddTicks(1380));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovimentacaoId",
                table: "movimentacoes_parcelas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4298));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4397));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4407));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4410));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4422));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4423));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4426));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4523));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 5, 31, 13, 28, 37, 158, DateTimeKind.Local).AddTicks(4531));

            migrationBuilder.CreateIndex(
                name: "IX_movimentacoes_parcelas_MovimentacaoId",
                table: "movimentacoes_parcelas",
                column: "MovimentacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_parcelas_movimentacoes_MovimentacaoId",
                table: "movimentacoes_parcelas",
                column: "MovimentacaoId",
                principalTable: "movimentacoes",
                principalColumn: "id");
        }
    }
}
