using Microsoft.EntityFrameworkCore;
using Movimentacoes.DTOS;
using Movimentacoes.Models;
using Resumos.Models;

namespace Infra.Database  {
public class Context : DbContext {
    
        public required DbSet<Movimentacao> Movimentacoes { get; set; }
        public required DbSet<MovimentacaoCategoria> MovimentacoesCategorias { get; set; }
        public required DbSet<MovimentacaoParcela> MovimentacaoParcelas { get; set; }

        public required DbSet<Resumo> Resumos { get; set; }
        public required DbSet<ResumoHistorico> ResumoHistoricos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movimentacao>()
                .Property(m => m.Tipo)
                .HasConversion<string>();


            modelBuilder.Entity<MovimentacaoCategoria>()
                .Property(c => c.Tipo)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovimentacaoCategoria>().HasData(
                new MovimentacaoCategoria("Comida e Mercado", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.COMIDA_MERCADO},
                new MovimentacaoCategoria("Educação e Desenvolvimento", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.EDUCACAO_DESENVOLVIMENTO},
                new MovimentacaoCategoria("Investimentos", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.INVESTIMENTOS},
                new MovimentacaoCategoria("Lazer e Bem-estar", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.LAZER_BEM_ESTAR},
                new MovimentacaoCategoria("Serviços", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.SERVICOS},
                new MovimentacaoCategoria("Moradia", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.MORADIA},
                new MovimentacaoCategoria("Transporte", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.TRANSPORTE},
                new MovimentacaoCategoria("Saúde", MovimentacaoTipo.SAIDA) {Id = (int) MovimentacaoCategoriaDTO.SAUDE},

                new MovimentacaoCategoria("Salário", MovimentacaoTipo.ENTRADA) {Id = (int) MovimentacaoCategoriaDTO.SALARIO},
                new MovimentacaoCategoria("Dividendo", MovimentacaoTipo.ENTRADA) {Id = (int) MovimentacaoCategoriaDTO.DIVIDENDO},
                new MovimentacaoCategoria("Venda", MovimentacaoTipo.ENTRADA) {Id = (int) MovimentacaoCategoriaDTO.VENDA},
                new MovimentacaoCategoria("Transferências", MovimentacaoTipo.ENTRADA) {Id = (int) MovimentacaoCategoriaDTO.TRANSFERENCIAS},
                new MovimentacaoCategoria("Outros", MovimentacaoTipo.ENTRADA) {Id = (int) MovimentacaoCategoriaDTO.OUTROS}
            );
            modelBuilder.Entity<Resumo>().HasData(
                new Resumo(0, "Corrente") {Id = 1},
                new Resumo(0, "Investimentos") {Id = 2}
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=finndb.sqlite");
            base.OnConfiguring(optionsBuilder);
        }

    
}}