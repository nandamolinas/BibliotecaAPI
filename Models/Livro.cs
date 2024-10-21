public class Livro
{
    public int IdLivro { get; set; } // Propriedade da chave primária
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoDePublicacao { get; set; }
    public DateTime? DataDeEmprestimo { get; set; }
    public DateTime? DataDeDevolucao { get; set; }
    public int? ClienteId { get; set; }

    public Livro() { }

    public Livro(string titulo, string autor)
    {
        Titulo = titulo;
        Autor = autor;
    }
}


