﻿// <auto-generated />
using System;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20250220000925_Resumo")]
    partial class Resumo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("Movimentacoes.Models.Movimentacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("categoria_id");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("tipo");

                    b.Property<float>("Valor")
                        .HasColumnType("REAL")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("movimentacoes");
                });

            modelBuilder.Entity("Movimentacoes.Models.MovimentacaoCategoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<DateTime?>("DeletadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("deletadoEm");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("nome");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("tipo");

                    b.HasKey("Id");

                    b.ToTable("movimentacoes_categorias");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5343),
                            Nome = "Comida e Mercado",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 2,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5380),
                            Nome = "Educação e Desenvolvimento",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 3,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5381),
                            Nome = "Investimentos",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 4,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5383),
                            Nome = "Lazer e Bem-estar",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 5,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5384),
                            Nome = "Serviços",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 6,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5385),
                            Nome = "Moradia",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 7,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5387),
                            Nome = "Transporte",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 8,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5388),
                            Nome = "Saúde",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 9,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5389),
                            Nome = "Salário",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 10,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5390),
                            Nome = "Dividendo",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 11,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5392),
                            Nome = "Venda",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 12,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5393),
                            Nome = "Transferências",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 13,
                            CriadoEm = new DateTime(2025, 2, 19, 20, 9, 24, 909, DateTimeKind.Local).AddTicks(5394),
                            Nome = "Outros",
                            Tipo = "ENTRADA"
                        });
                });

            modelBuilder.Entity("Movimentacoes.Models.MovimentacaoParcela", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<int>("MovimentacaoId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("movimentacao_id");

                    b.Property<int>("Numero")
                        .HasColumnType("INTEGER")
                        .HasColumnName("numero");

                    b.Property<float>("Valor")
                        .HasColumnType("REAL")
                        .HasColumnName("valor");

                    b.Property<DateTime>("Vencimento")
                        .HasColumnType("TEXT")
                        .HasColumnName("vencimento");

                    b.HasKey("Id");

                    b.HasIndex("MovimentacaoId");

                    b.ToTable("movimentacoes_parcelas");
                });

            modelBuilder.Entity("Movimentacoes.Models.Resumo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("nome");

                    b.Property<float>("Valor")
                        .HasColumnType("REAL")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.ToTable("resumo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nome = "Corrente",
                            Valor = 0f
                        },
                        new
                        {
                            Id = 2,
                            Nome = "Investimentos",
                            Valor = 0f
                        });
                });

            modelBuilder.Entity("Movimentacoes.Models.ResumoHistorico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<int>("ResumoId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("resumoId");

                    b.Property<float>("Valor")
                        .HasColumnType("REAL")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("ResumoId");

                    b.ToTable("resumo_historico");
                });

            modelBuilder.Entity("Movimentacoes.Models.Movimentacao", b =>
                {
                    b.HasOne("Movimentacoes.Models.MovimentacaoCategoria", "Categoria")
                        .WithMany("Movimentacoes")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Movimentacoes.Models.MovimentacaoParcela", b =>
                {
                    b.HasOne("Movimentacoes.Models.Movimentacao", "Movimentacao")
                        .WithMany("MovimentacaoParcelas")
                        .HasForeignKey("MovimentacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movimentacao");
                });

            modelBuilder.Entity("Movimentacoes.Models.ResumoHistorico", b =>
                {
                    b.HasOne("Movimentacoes.Models.Resumo", "Resumo")
                        .WithMany("ResumoHistorico")
                        .HasForeignKey("ResumoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resumo");
                });

            modelBuilder.Entity("Movimentacoes.Models.Movimentacao", b =>
                {
                    b.Navigation("MovimentacaoParcelas");
                });

            modelBuilder.Entity("Movimentacoes.Models.MovimentacaoCategoria", b =>
                {
                    b.Navigation("Movimentacoes");
                });

            modelBuilder.Entity("Movimentacoes.Models.Resumo", b =>
                {
                    b.Navigation("ResumoHistorico");
                });
#pragma warning restore 612, 618
        }
    }
}
