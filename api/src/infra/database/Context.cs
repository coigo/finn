using Microsoft.EntityFrameworkCore;
using Movimentacoes.Models;

namespace Infra.Database  {
public class Context : DbContext {
        public required DbSet<Movimentacao> Movimentacoes { get; set; }
        public required DbSet<MovimentacaoCategoria> MovimentacoesCategorias { get; set; }
        public required DbSet<MovimentacaoParcela> MovimentacaoParcelas { get; set; }

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
                new MovimentacaoCategoria("Comida e Mercado", MovimentacaoTipo.SAIDA) {Id = 1},
                new MovimentacaoCategoria("Educação e Desenvolvimento", MovimentacaoTipo.SAIDA) {Id = 2},
                new MovimentacaoCategoria("Investimentos", MovimentacaoTipo.SAIDA) {Id = 3},
                new MovimentacaoCategoria("Lazer e Bem-estar", MovimentacaoTipo.SAIDA) {Id = 4},
                new MovimentacaoCategoria("Serviços", MovimentacaoTipo.SAIDA) {Id = 5},
                new MovimentacaoCategoria("Moradia", MovimentacaoTipo.SAIDA) {Id = 6},
                new MovimentacaoCategoria("Transporte", MovimentacaoTipo.SAIDA) {Id = 7},
                new MovimentacaoCategoria("Saúde", MovimentacaoTipo.SAIDA) {Id = 8},

                new MovimentacaoCategoria("Salário", MovimentacaoTipo.ENTRADA) {Id = 9},
                new MovimentacaoCategoria("Dividendo", MovimentacaoTipo.ENTRADA) {Id = 10},
                new MovimentacaoCategoria("Venda", MovimentacaoTipo.ENTRADA) {Id = 11},
                new MovimentacaoCategoria("Transferências", MovimentacaoTipo.ENTRADA) {Id = 12},
                new MovimentacaoCategoria("Outros", MovimentacaoTipo.ENTRADA) {Id = 13}
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=finndb.sqlite");
            base.OnConfiguring(optionsBuilder);
        }

    
}}