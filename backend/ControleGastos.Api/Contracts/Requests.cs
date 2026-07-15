using System.ComponentModel.DataAnnotations;
using ControleGastos.Api.Domain;

namespace ControleGastos.Api.Contracts;

public sealed class CriarPessoaRequest
{
    [Required, StringLength(120)] public string Nome { get; init; } = string.Empty;
    [Range(0, 150)] public int Idade { get; init; }
}

public sealed class CriarTransacaoRequest
{
    [Required, StringLength(250)] public string Descricao { get; init; } = string.Empty;
    [Range(typeof(decimal), "0.01", "9999999999999999")] public decimal Valor { get; init; }
    [Required] public TipoTransacao? Tipo { get; init; }
    [Required] public Guid? PessoaId { get; init; }
}
