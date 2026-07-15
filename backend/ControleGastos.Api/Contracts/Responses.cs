using ControleGastos.Api.Domain;

namespace ControleGastos.Api.Contracts;

public sealed record PessoaResponse(Guid Id, string Nome, int Idade);
public sealed record TransacaoResponse(Guid Id, string Descricao, decimal Valor, TipoTransacao Tipo, Guid PessoaId, string PessoaNome);
public sealed record TotalPessoaResponse(Guid PessoaId, string Nome, decimal Receitas, decimal Despesas, decimal Saldo);
public sealed record TotaisResponse(IReadOnlyList<TotalPessoaResponse> Pessoas, decimal TotalReceitas, decimal TotalDespesas, decimal SaldoLiquido);
