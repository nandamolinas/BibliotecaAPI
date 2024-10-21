using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Models
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Cliente> Clientes { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emprestimo>()
                .HasKey(e => e.IdEmprestimo); 

            modelBuilder.Entity<Livro>()
                .HasKey(l => l.IdLivro); 

           
        }
    }
}
