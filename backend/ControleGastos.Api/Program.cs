using ControleGastos.Api.Contracts;
using ControleGastos.Api.Data;
using ControleGastos.Api.Domain;
using ControleGastos.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddDbContext<GastosDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Gastos")
        ?? "Data Source=data/gastos.db"));
builder.Services.AddDbContext<GastosDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Gastos") ?? "Data Source=data/gastos.db"));
builder.Services.AddScoped<TransacaoService>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

// Cria o arquivo e as tabelas na primeira execução, mantendo os dados nas execuções seguintes.
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<GastosDbContext>().Database.EnsureCreatedAsync();

app.MapGet("/api/pessoas", async (GastosDbContext db) =>
    await db.Pessoas.AsNoTracking().OrderBy(p => p.Nome)
        .Select(p => new PessoaResponse(p.Id, p.Nome, p.Idade)).ToListAsync());

app.MapPost("/api/pessoas", async (CriarPessoaRequest request, GastosDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Nome)) return Results.ValidationProblem(new Dictionary<string, string[]> { ["nome"] = ["Nome é obrigatório."] });
    var pessoa = new Pessoa { Nome = request.Nome.Trim(), Idade = request.Idade };
    db.Pessoas.Add(pessoa);
    await db.SaveChangesAsync();
    return Results.Created($"/api/pessoas/{pessoa.Id}", new PessoaResponse(pessoa.Id, pessoa.Nome, pessoa.Idade));
});

app.MapDelete("/api/pessoas/{id:guid}", async (Guid id, GastosDbContext db) =>
{
    var pessoa = await db.Pessoas.FindAsync(id);
    if (pessoa is null) return Results.NotFound();
    db.Pessoas.Remove(pessoa); // A relação configurada no contexto remove as transações associadas.
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/api/transacoes", async (GastosDbContext db) =>
    await db.Transacoes.AsNoTracking().Include(t => t.Pessoa).OrderByDescending(t => t.Id)
        .Select(t => new TransacaoResponse(t.Id, t.Descricao, t.Valor, t.Tipo, t.PessoaId, t.Pessoa!.Nome)).ToListAsync());

app.MapPost("/api/transacoes", async (CriarTransacaoRequest request, TransacaoService service, GastosDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Descricao) || request.Valor <= 0 || request.Tipo is null || request.PessoaId is null)
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["transacao"] = ["Preencha descrição, valor positivo, tipo e pessoa."] });
    var (transacao, erro) = await service.CriarAsync(request);
    if (erro is not null) return Results.BadRequest(new { mensagem = erro });
    var nome = await db.Pessoas.Where(p => p.Id == transacao!.PessoaId).Select(p => p.Nome).SingleAsync();
    return Results.Created($"/api/transacoes/{transacao.Id}", new TransacaoResponse(transacao.Id, transacao.Descricao, transacao.Valor, transacao.Tipo, transacao.PessoaId, nome));
});

app.MapGet("/api/totais", async (GastosDbContext db) =>
{
    var pessoasDb = await db.Pessoas
        .Include(p => p.Transacoes)
        .AsNoTracking()
        .OrderBy(p => p.Nome)
        .ToListAsync();

    var pessoas = pessoasDb.Select(p => new TotalPessoaResponse(
        p.Id,
        p.Nome,
        p.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Receita)
            .Sum(t => t.Valor),
        p.Transacoes
            .Where(t => t.Tipo == TipoTransacao.Despesa)
            .Sum(t => t.Valor),
        0
    )).ToList();

    var totaisComSaldo = pessoas
        .Select(p => p with { Saldo = p.Receitas - p.Despesas })
        .ToList();

    var receitas = totaisComSaldo.Sum(p => p.Receitas);
    var despesas = totaisComSaldo.Sum(p => p.Despesas);

    return new TotaisResponse(
        totaisComSaldo,
        receitas,
        despesas,
        receitas - despesas
    );
});


app.Run();

public partial class Program { }
