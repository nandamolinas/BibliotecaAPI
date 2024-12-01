using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Data;

public class AppDataContext : DbContext
{
    // Declarar os DbSets (tabelas do banco de dados)
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }

    // Configurar o SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Biblioteca.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Emprestimo>()
        .HasOne(e => e.Livro)
        .WithMany()
        .HasForeignKey(e => e.LivroId)
        .HasPrincipalKey(l => l.LivroId); // Configura a chave principal para a relação

    modelBuilder.Entity<Emprestimo>()
        .HasOne(e => e.Cliente)
        .WithMany()
        .HasForeignKey(e => e.ClienteId);
}

}
