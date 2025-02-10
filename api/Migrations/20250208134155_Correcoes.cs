using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Correcoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_MovimentacoesCategorias_CategoriaId",
                table: "Movimentacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movimentacoes",
                table: "Movimentacoes");

            migrationBuilder.RenameTable(
                name: "Movimentacoes",
                newName: "movimentacao");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "movimentacao",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "movimentacao",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Movimentacoes_CategoriaId",
                table: "movimentacao",
                newName: "IX_movimentacao_CategoriaId");

            migrationBuilder.AlterColumn<float>(
                name: "Valor",
                table: "movimentacao",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movimentacao",
                table: "movimentacao",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MovimentacaoParcelas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovimentacaoIdId = table.Column<int>(type: "INTEGER", nullable: false),
                    Vumero = table.Column<int>(type: "INTEGER", nullable: false),
                    Valor = table.Column<float>(type: "REAL", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacaoParcelas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacaoParcelas_movimentacao_MovimentacaoIdId",
                        column: x => x.MovimentacaoIdId,
                        principalTable: "movimentacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoParcelas_MovimentacaoIdId",
                table: "MovimentacaoParcelas",
                column: "MovimentacaoIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacao_MovimentacoesCategorias_CategoriaId",
                table: "movimentacao",
                column: "CategoriaId",
                principalTable: "MovimentacoesCategorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacao_MovimentacoesCategorias_CategoriaId",
                table: "movimentacao");

            migrationBuilder.DropTable(
                name: "MovimentacaoParcelas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movimentacao",
                table: "movimentacao");

            migrationBuilder.RenameTable(
                name: "movimentacao",
                newName: "Movimentacoes");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Movimentacoes",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Movimentacoes",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_movimentacao_CategoriaId",
                table: "Movimentacoes",
                newName: "IX_Movimentacoes_CategoriaId");

            migrationBuilder.AlterColumn<int>(
                name: "valor",
                table: "Movimentacoes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movimentacoes",
                table: "Movimentacoes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_MovimentacoesCategorias_CategoriaId",
                table: "Movimentacoes",
                column: "CategoriaId",
                principalTable: "MovimentacoesCategorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
