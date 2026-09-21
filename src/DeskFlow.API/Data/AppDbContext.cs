
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Chamado> Chamados => Set<Chamado>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chamado>().HasOne (chamado => chamado.Categoria)
            .WithMany (categoria => categoria.Chamados) .HasForeignKey (chamado => chamado.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}