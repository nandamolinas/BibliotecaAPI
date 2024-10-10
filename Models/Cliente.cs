using System;

namespace BibliotecaAPI.Models;

public class Cliente
{
public Cliente()
    {
        Id = Guid.NewGuid().ToString();
        DataDeInicio= DateTime.Now;
    }

    public string? Id{ get; set;}
    public string? Nome{ get; set;}
    public string? Cpf{ get; set;}
    public DateTime DataDeInicio{ get; set;}




}
