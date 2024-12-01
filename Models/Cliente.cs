using System;

namespace BibliotecaAPI.Models;

public class Cliente
{
    public Cliente()
    {
        DataDeInicio = DateTime.Now;
    }

    public int ClienteId { get; set; } // Propriedade não-nullable para chave primária
    public string Nome { get; set; } // Propriedade obrigatória
    public string Cpf { get; set; } // Propriedade obrigatória
    public DateTime DataDeInicio { get; set; } // Data automática
}

