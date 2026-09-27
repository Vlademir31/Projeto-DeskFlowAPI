
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Chamado> Chamados => Set<Chamado>();
        public DbSet<Interacao> Interacoes => Set<Interacao>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chamado>().HasOne (chamado => chamado.Categoria)
            .WithMany (categoria => categoria.Chamados) .HasForeignKey (chamado => chamado.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Interacao>().HasOne(Interacao => Interacao.Chamado)
            .WithMany(chamado => chamado.Interacoes).HasForeignKey (Interacao => Interacao.ChamadoId);
        }
    }
}