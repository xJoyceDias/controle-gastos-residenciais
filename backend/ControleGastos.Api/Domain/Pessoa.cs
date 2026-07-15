namespace ControleGastos.Api.Domain;

/// <summary>Representa uma pessoa que pode ter lançamentos financeiros.</summary>
public sealed class Pessoa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public List<Transacao> Transacoes { get; set; } = [];
}
