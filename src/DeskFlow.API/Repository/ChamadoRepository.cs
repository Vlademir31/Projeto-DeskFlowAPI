using DeskFlow.API.Data;
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repository
{
    public class ChamadoRepository : IChamadoRepository
    {
     private readonly AppDbContext _context;
     public ChamadoRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Chamado>> ObterTodosAsync(StatusChamado? status, Prioridade? prioridade,
        int? categoriaId)
        {
            var consulta = _context.Chamados.Include(chamado => chamado.Categoria).AsQueryable();

            if (status.HasValue)
            {
                consulta = consulta.Where(chamado => chamado.Status == status.Value);
            }

            if (prioridade.HasValue)
            {
                consulta = consulta.Where( chamado => chamado.Prioridade == prioridade.Value);
            }

            if (categoriaId.HasValue)
            {
                consulta = consulta.Where(chamado => chamado.CategoriaId == categoriaId.Value);
            }

            return await consulta.AsNoTracking().ToListAsync();
        }

        public async Task<Chamado?> ObterPorIdAsync(int id)
        {
            return await _context.Chamados.Include(chamado => chamado.Categoria)
            .Include(chamado => chamado.Interacoes).FirstOrDefaultAsync(Chamado => Chamado.Id == id);
        }
        public async Task AdicionarAsync (Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);

            await _context.SaveChangesAsync();
        }
        public async Task AtualizarAsync (Chamado chamado)
        {
            _context.Chamados.Update(chamado);

            await _context.SaveChangesAsync();
        }
        
    }
}