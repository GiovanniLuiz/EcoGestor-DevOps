using EcoGestorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoGestorApi.Data;

public class EcoGestorDbContext : DbContext
{
    public EcoGestorDbContext(DbContextOptions<EcoGestorDbContext> options) : base(options) { }

    public DbSet<Unidade> Unidades => Set<Unidade>();
    public DbSet<PontoColeta> PontosColeta => Set<PontoColeta>();
    public DbSet<ColetaResiduo> ColetasResiduo => Set<ColetaResiduo>();
    public DbSet<DestinacaoResiduo> DestinacoesResiduo => Set<DestinacaoResiduo>();
    public DbSet<AlertaColeta> AlertasColeta => Set<AlertaColeta>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Unidade>()
            .HasMany(u => u.PontosColeta)
            .WithOne(p => p.Unidade)
            .HasForeignKey(p => p.IdUnidade)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PontoColeta>()
            .HasMany(p => p.ColetasResiduo)
            .WithOne(c => c.PontoColeta)
            .HasForeignKey(c => c.IdPonto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PontoColeta>()
            .HasMany(p => p.AlertasColeta)
            .WithOne(a => a.PontoColeta)
            .HasForeignKey(a => a.IdPonto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ColetaResiduo>()
            .HasOne(c => c.DestinacaoResiduo)
            .WithOne(d => d.ColetaResiduo)
            .HasForeignKey<DestinacaoResiduo>(d => d.IdColeta)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Unidade>().HasData(
            new Unidade { IdUnidade = 1, NomeUnidade = "Unidade Centro", Cidade = "São Paulo", Responsavel = "Giovanni Luiz", MetaReciclagemKg = 500 },
            new Unidade { IdUnidade = 2, NomeUnidade = "Unidade Jundiaí", Cidade = "Jundiaí", Responsavel = "Equipe ESG", MetaReciclagemKg = 350 }
        );

        modelBuilder.Entity<PontoColeta>().HasData(
            new PontoColeta { IdPonto = 1, IdUnidade = 1, TipoResiduo = "PLASTICO", CapacidadeMaxKg = 100, NivelAtualKg = 75, StatusPonto = "ATENCAO" },
            new PontoColeta { IdPonto = 2, IdUnidade = 2, TipoResiduo = "PAPEL", CapacidadeMaxKg = 80, NivelAtualKg = 68, StatusPonto = "CRITICO" }
        );

        modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
    }
}
