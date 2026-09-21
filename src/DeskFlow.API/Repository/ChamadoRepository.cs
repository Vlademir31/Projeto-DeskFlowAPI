using DeskFlow.API.Data;
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repository
{
    public class ChamadoRepository : IChamadoRepository
    {
     private readonly AppDbContext _context;
     public ChamadoRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync (Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);

            await _context.SaveChangesAsync();
        }
    }
}