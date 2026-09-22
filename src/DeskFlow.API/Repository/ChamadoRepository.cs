using DeskFlow.API.Data;
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
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
        public async Task<Chamado?> ObterPorIdAsync(int id)
        {
            return await _context.Chamados.FirstOrDefaultAsync(Chamado => Chamado.Id == id);
        }
        public async Task AdicionarAsync (Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);

            await _context.SaveChangesAsync();
        }
        
    }
}