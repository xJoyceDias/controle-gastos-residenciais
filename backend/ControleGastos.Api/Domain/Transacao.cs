namespace ControleGastos.Api.Domain;

/// <summary>Um lançamento de receita ou despesa ligado a uma pessoa.</summary>
public sealed class Transacao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public Guid PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }
}
