public class Cliente
{
    public int ClienteId { get; set; }
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
    public DateTime DataDeInicio { get; set; } = DateTime.Now;
}
