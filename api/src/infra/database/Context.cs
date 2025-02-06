using Microsoft.EntityFrameworkCore;
using Model.Movimentacao;

namespace Infra.Database  {
public class Context : DbContext {
        public required DbSet<MovimentacaoModel> Movimentacoes { get; set; }
        public required DbSet<MovimentacaoCategoriaModel> MovimentacoesCategorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=finndb.sqlite");
            base.OnConfiguring(optionsBuilder);
        }
}}