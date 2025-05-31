using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MovimentacoesPersistentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_parcelas_movimentacoes_movimentacao_id",
                table: "movimentacoes_parcelas");

            migrationBuilder.RenameColumn(
                name: "movimentacao_id",
                table: "movimentacoes_parcelas",
                newName: "MovimentacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_movimentacoes_parcelas_movimentacao_id",
                table: "movimentacoes_parcelas",
                newName: "IX_movimentacoes_parcelas_MovimentacaoId");

            migrationBuilder.AlterColumn<int>(
                name: "MovimentacaoId",
                table: "movimentacoes_parcelas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "descricao",
                table: "movimentacoes_parcelas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "paga",
                table: "movimentacoes_parcelas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "rotina",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rotina", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1235));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1277));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1278));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 5,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1279));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 6,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1280));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 7,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1281));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 8,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1282));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 9,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1284));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 10,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1285));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 11,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1286));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 12,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1287));

            migrationBuilder.UpdateData(
                table: "movimentacoes_categorias",
                keyColumn: "id",
                keyValue: 13,
                column: "criadoEm",
                value: new DateTime(2025, 5, 28, 20, 39, 51, 821, DateTimeKind.Local).AddTicks(1288));

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_parcelas_movimentacoes_MovimentacaoId",
                table: "movimentacoes_parcelas",
                column: "MovimentacaoId",
                principalTable: "movimentacoes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacoes_parcelas_movimentacoes_MovimentacaoId",
                table: "movimentacoes_parcelas");

            migrationBuilder.DropTable(
                name: "rotina");

            migrationBuilder.DropColumn(
                name: "descricao",
                table: "movimentacoes_parcelas");

            migrationBuilder.DropColumn(
                name: "paga",
                table: "movimentacoes_parcelas");

            migrationBuilder.RenameColumn(
                name: "MovimentacaoId",
                table: "movimentacoes_parcelas",
                newName: "movimentacao_id");

            migrationBuilder.RenameIndex(
                name: "IX_movimentacoes_parcelas_MovimentacaoId",
                table: "movimentacoes_parcelas",
                newName: "IX_movimentacoes_parcelas_movimentacao_id");

            migrationBuilder.AlterColumn<int>(
                name: "movimentacao_id",
                table: "movimentacoes_parcelas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacoes_parcelas_movimentacoes_movimentacao_id",
                table: "movimentacoes_parcelas",
                column: "movimentacao_id",
                principalTable: "movimentacoes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
