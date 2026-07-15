using ControleGastos.Api.Contracts;
using ControleGastos.Api.Data;
using ControleGastos.Api.Domain;
using ControleGastos.Api.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ControleGastos.Tests;

public sealed class TransacaoServiceTests : IDisposable
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");
    private readonly GastosDbContext _db;
    public TransacaoServiceTests()
    {
        _connection.Open();
        _db = new GastosDbContext(new DbContextOptionsBuilder<GastosDbContext>().UseSqlite(_connection).Options);
        _db.Database.EnsureCreated();
    }

    [Fact]
    public async Task Menor_de_idade_nao_pode_cadastrar_receita()
    {
        var pessoa = new Pessoa { Nome = "Ana", Idade = 17 }; _db.Pessoas.Add(pessoa); await _db.SaveChangesAsync();
        var (transacao, erro) = await new TransacaoService(_db).CriarAsync(new CriarTransacaoRequest { Descricao = "Mesada", Valor = 50, Tipo = TipoTransacao.Receita, PessoaId = pessoa.Id });
        Assert.Null(transacao); Assert.Contains("menores", erro);
    }

    [Fact]
    public async Task Pessoa_inexistente_nao_pode_receber_transacao()
    {
        var (transacao, erro) = await new TransacaoService(_db).CriarAsync(new CriarTransacaoRequest { Descricao = "Conta", Valor = 50, Tipo = TipoTransacao.Despesa, PessoaId = Guid.NewGuid() });
        Assert.Null(transacao); Assert.Contains("não existe", erro);
    }
    public void Dispose() { _db.Dispose(); _connection.Dispose(); }
}
