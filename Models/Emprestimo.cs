using System.Text.Json.Serialization;
using BibliotecaAPI.Models;

public class Emprestimo
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int LivroId { get; set; }
    public DateTime DataDeEmprestimo { get; set; }
    public DateTime? DataDeDevolucao { get; set; }

    public Cliente Cliente { get; set; }
    public Livro Livro { get; set; }

    [JsonConstructor]
    public Emprestimo(int clienteId, int livroId, DateTime dataDeEmprestimo)
    {
        ClienteId = clienteId;
        LivroId = livroId;
        DataDeEmprestimo = dataDeEmprestimo;
    }
}
