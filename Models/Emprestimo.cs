public class Emprestimo
{
    public int IdEmprestimo { get; set; }
    public string Titulo { get; set; }
    public string ClienteNome { get; set; }
    public DateTime? DataDeEmprestimo { get; set; }
    public DateTime? DataDeDevolucao { get; set; }
}
