namespace BibliotecaAPI.Models
{
    public class Livro
    {
        public Livro() { }

        public int LivroId { get; set; } // Chave primária
        public string? Titulo { get; set; } // Título do livro
        public string? Autor { get; set; } // Autor do livro
        public int AnoDePublicacao { get; set; } // Ano de publicação
    }
}
