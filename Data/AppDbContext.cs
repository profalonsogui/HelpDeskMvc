using HelpDeskMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskMvc.Data;

public class AppDbContext : DbContext
{
    // O DbContext é o "coração" do Entity Framework Core. Ele representa a sessão com o banco de dados e permite consultar e salvar dados.
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Chamado> Chamados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Persistimos o enum como texto (ex.: "Aberto") em vez de número,
        // mantendo compatibilidade com os registros já gravados no SQLite
        // e a legibilidade direta do valor na tabela.
        modelBuilder.Entity<Chamado>()
            .Property(c => c.Status)
            .HasConversion<string>();
    }
}