using System;

namespace BibliotecaAPI.Models
{
    public class Emprestimo
    {
        public int IdEmprestimo { get; set; }
        public int ClienteId { get; set; } // Referência ao Cliente
        public int IdLivro { get; set; } // Referência ao Livro
        public DateTime DataDeEmprestimo { get; set; } // Data do Empréstimo
        public DateTime DataDeDevolucao { get; set; } // Data da Devolução
    }
}
