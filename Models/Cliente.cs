using System;

namespace BibliotecaAPI.Models;

public class Cliente
{
public Cliente()
    {
        DataDeInicio= DateTime.Now;
    }

    public int? ClienteId{ get; set;}
    public string? Nome{ get; set;}
    public string? Cpf{ get; set;}
    public DateTime DataDeInicio{ get; set;}




}
