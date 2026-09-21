using DeskFlow.API.Data;
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskFlow.API.Repository;
public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;
    public CategoriaRepository (AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Categoria>> ObterTodosAsync()
    {
        return await _context.Categorias.AsNoTracking().ToListAsync();
    }
    public async Task<Categoria?> ObterPorIdAsync (int id)
    {
        return await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(categoria =>
        categoria.Id == id);
    }
    public async Task AdicionarAsync (Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }
    public async Task AtualizarAsync (Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }
    public async Task RemoverAsync (Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }

}