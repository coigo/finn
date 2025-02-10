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
                new MovimentacaoCategoria("Comida", MovimentacaoTipo.SAIDA) {Id = 1},
                new MovimentacaoCategoria("Educação", MovimentacaoTipo.SAIDA) {Id = 2}
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=finndb.sqlite");
            base.OnConfiguring(optionsBuilder);
        }

    
}}