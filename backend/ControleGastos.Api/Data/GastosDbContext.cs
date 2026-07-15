using ControleGastos.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Api.Data;

/// <summary>Centraliza o mapeamento do banco e garante exclusão em cascata.</summary>
public sealed class GastosDbContext(DbContextOptions<GastosDbContext> options) : DbContext(options)
{
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.ToTable("Pessoas");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).HasMaxLength(120).IsRequired();
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.ToTable("Transacoes");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Descricao).HasMaxLength(250).IsRequired();
            entity.Property(t => t.Valor).HasPrecision(18, 2);
            entity.Property(t => t.Tipo).HasConversion<string>().HasMaxLength(10);
            // A exclusão em cascata atende à regra: ao apagar uma pessoa, apaga seus lançamentos.
            entity.HasOne(t => t.Pessoa)
                .WithMany(p => p.Transacoes)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
