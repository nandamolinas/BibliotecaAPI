using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace API.Models;

//Implementar a henraça da classe DbContext
public class AppDataContext : DbContext
{
    //Declarar todas as classes de modelo que vão virar tabelas no banco de dados
    public DbSet<Cliente> Clientes{ get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }

    //Qual o banco de dados que será utilizado
    //String de conexão
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Biblioteca.db");
    }
}
