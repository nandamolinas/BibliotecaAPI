public class Livro
{
    public Livro()
    {
        Id = Guid.NewGuid().ToString();
        CriadoEm = DateTime.Now;
    }

    public string Id { get; set; }          
    
    public string? Titulo { get; set; }   
    
    public string? Autor { get; set; }   
    
    public int AnoPublicacao { get; set; }   
    
    public string? Genero { get; set; } 
    
    public int Quantidade { get; set; } 
    DateTime CriadoEm { get; set; }

    
    
}