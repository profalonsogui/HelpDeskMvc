using HelpDeskMvc.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // O DbContext é o "coração" do Entity Framework Ele representa a sessão com o BD, 
    // E permite consultar e salvar dados.
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        
    }

    public DbSet<Chamado> Chamados { get; set; }
}