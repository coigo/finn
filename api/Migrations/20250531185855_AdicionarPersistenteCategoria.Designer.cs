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
    [Migration("20250531185855_AdicionarPersistenteCategoria")]
    partial class AdicionarPersistenteCategoria
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("Aportes.Models.Aporte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("categoria");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<string>("Identificador")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("identificador");

                    b.Property<decimal>("PrecoMedio")
                        .HasColumnType("TEXT")
                        .HasColumnName("precoMedio");

                    b.Property<decimal>("Quantidade")
                        .HasColumnType("TEXT")
                        .HasColumnName("quantidade");

                    b.HasKey("Id");

                    b.HasAlternateKey("Identificador");

                    b.ToTable("aportes");
                });

            modelBuilder.Entity("Aportes.Models.AporteHistorico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("categoria");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<DateTime>("Data")
                        .HasColumnType("TEXT")
                        .HasColumnName("data");

                    b.Property<string>("Identificador")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("identificador");

                    b.Property<decimal>("Preco")
                        .HasColumnType("TEXT")
                        .HasColumnName("precoMedio");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("tipo");

                    b.HasKey("Id");

                    b.HasIndex("Identificador");

                    b.ToTable("aportes_historico");
                });

            modelBuilder.Entity("Movimentacoes.Models.Movimentacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("categoriaId");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<DateTime>("Data")
                        .HasColumnType("TEXT")
                        .HasColumnName("data");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("descricao");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("tipo");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT")
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
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6599),
                            Nome = "Comida e Mercado",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 2,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6651),
                            Nome = "Educação e Desenvolvimento",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 3,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6653),
                            Nome = "Investimentos",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 4,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6654),
                            Nome = "Lazer e Bem-estar",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 5,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6655),
                            Nome = "Serviços",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 6,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6656),
                            Nome = "Moradia",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 7,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6658),
                            Nome = "Transporte",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 8,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6659),
                            Nome = "Saúde",
                            Tipo = "SAIDA"
                        },
                        new
                        {
                            Id = 9,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6660),
                            Nome = "Salário",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 10,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6661),
                            Nome = "Dividendo",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 11,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6663),
                            Nome = "Venda",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 12,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6664),
                            Nome = "Transferências",
                            Tipo = "ENTRADA"
                        },
                        new
                        {
                            Id = 13,
                            CriadoEm = new DateTime(2025, 5, 31, 14, 58, 55, 422, DateTimeKind.Local).AddTicks(6665),
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

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("descricao");

                    b.Property<int>("Numero")
                        .HasColumnType("INTEGER")
                        .HasColumnName("numero");

                    b.Property<bool>("Paga")
                        .HasColumnType("INTEGER")
                        .HasColumnName("paga");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT")
                        .HasColumnName("valor");

                    b.Property<DateTime>("Vencimento")
                        .HasColumnType("TEXT")
                        .HasColumnName("vencimento");

                    b.HasKey("Id");

                    b.ToTable("movimentacoes_parcelas");
                });

            modelBuilder.Entity("Movimentacoes.Models.MovimentacaoPersistente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("categoriaId");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("descricao");

                    b.Property<int>("Tipo")
                        .HasColumnType("INTEGER")
                        .HasColumnName("tipo");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.ToTable("movimentacoes_persistentes");
                });

            modelBuilder.Entity("Resumos.Models.Resumo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("nome");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.ToTable("resumo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nome = "Corrente",
                            Valor = 0m
                        },
                        new
                        {
                            Id = 2,
                            Nome = "Investimentos",
                            Valor = 0m
                        });
                });

            modelBuilder.Entity("Resumos.Models.ResumoHistorico", b =>
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

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("ResumoId");

                    b.ToTable("resumo_historico");
                });

            modelBuilder.Entity("Rotinas.Models.Rotina", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("Data")
                        .HasColumnType("TEXT")
                        .HasColumnName("data");

                    b.HasKey("Id");

                    b.ToTable("rotina");
                });

            modelBuilder.Entity("Salarios.Models.Salario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT")
                        .HasColumnName("criadoEm");

                    b.Property<decimal>("Valor")
                        .HasColumnType("TEXT")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.ToTable("salarios");
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

            modelBuilder.Entity("Resumos.Models.ResumoHistorico", b =>
                {
                    b.HasOne("Resumos.Models.Resumo", "Resumo")
                        .WithMany("ResumoHistorico")
                        .HasForeignKey("ResumoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resumo");
                });

            modelBuilder.Entity("Movimentacoes.Models.MovimentacaoCategoria", b =>
                {
                    b.Navigation("Movimentacoes");
                });

            modelBuilder.Entity("Resumos.Models.Resumo", b =>
                {
                    b.Navigation("ResumoHistorico");
                });
#pragma warning restore 612, 618
        }
    }
}
