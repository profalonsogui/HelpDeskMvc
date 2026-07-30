using HelpDeskMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskMvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Chamado> Chamados { get; set; }
}