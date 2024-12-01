using System;

namespace BibliotecaAPI.Models;

public class Livro
{
    public Livro() { }

    
    public string LivroId { get; set; } = Guid.NewGuid().ToString();    // Propriedade da chave primária
    public string? Titulo { get; set; } // Propriedade obrigatória
    public string? Autor { get; set; } // Propriedade obrigatória
    public int AnoDePublicacao { get; set; } // Ano obrigatório

    public DateTime? DataDeEmprestimo { get; set; } // Data de empréstimo opcional
    public DateTime? DataDeDevolucao { get; set; } // Data de devolução opcional

    public Cliente? Cliente { get; set; } // Referência ao Cliente
    public int? ClienteId { get; set; } // Chave estrangeira do Cliente
}

