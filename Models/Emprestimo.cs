using System;

namespace BibliotecaAPI.Models;

public class Emprestimo
{
    public int Id { get; set; } // Propriedade para chave primária
    public int ClienteId { get; set; } // Chave estrangeira do Cliente
    public int LivroId { get; set; } // Chave estrangeira do Livro
    public DateTime DataDeEmprestimo { get; set; } // Data obrigatória
    public DateTime? DataDeDevolucao { get; set; } // Data opcional para devolução
}
