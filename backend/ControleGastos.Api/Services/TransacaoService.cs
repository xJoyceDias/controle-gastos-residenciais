using ControleGastos.Api.Contracts;
using ControleGastos.Api.Data;
using ControleGastos.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Services;

/// <summary>Concentra as regras de negócio de criação de transações.</summary>
public sealed class TransacaoService(GastosDbContext db)
{
    public async Task<(Transacao? Transacao, string? Erro)> CriarAsync(CriarTransacaoRequest request)
    {
        var pessoa = await db.Pessoas.FindAsync(request.PessoaId!.Value);
        if (pessoa is null)
            return (null, "A pessoa informada não existe.");

        // Menores não podem registrar entrada de dinheiro, somente despesas.
        if (pessoa.Idade < 18 && request.Tipo == TipoTransacao.Receita)
            return (null, "Pessoas menores de 18 anos só podem cadastrar despesas.");

        var transacao = new Transacao
        {
            Descricao = request.Descricao.Trim(),
            Valor = request.Valor,
            Tipo = request.Tipo!.Value,
            PessoaId = pessoa.Id
        };
        db.Transacoes.Add(transacao);
        await db.SaveChangesAsync();
        return (transacao, null);
    }
}
