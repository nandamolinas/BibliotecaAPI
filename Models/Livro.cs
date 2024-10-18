using BibliotecaAPI.Models;

public class Livro
{
    public Livro()
    {
        Id = Guid.NewGuid().ToString();
        AdicionadoEm= DateTime.Now;
    }

   public string? Id{ get; set;}
    public string? Titulo{ get; set;}
    public string? Autor{ get; set;}
    public int? AnoDePublicacao{ get; set;}
    public string? DataDeEmprestimo{get; set;}
    public string? DataDeDevolucao{get;set;}   
    public DateTime AdicionadoEm{ get; set;}

    public Cliente? Cliente{get;set;}




    
}