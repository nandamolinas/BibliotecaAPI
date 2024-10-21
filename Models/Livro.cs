public class Livro
{
    public int IdLivro { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoDePublicacao { get; set; }
    public DateTime? DataDeEmprestimo { get; set; }
    public DateTime? DataDeDevolucao { get; set; }
    
     
    public int? ClienteId { get; set; }  
}
