using DeskFlow.API.Models.Entities;
using DeskFlow.API.Interfaces;

namespace DeskFlow.API.Services;

    public class CategoriaService 
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Categoria>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }
    public async Task<Categoria?> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }
    public async Task AdicionarAsync (Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nome))
        {
            throw new ArgumentException("O nome é obrigatório.");
        }

        await _repository.AdicionarAsync(categoria);
    }
    public async Task AtualizarAsync(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nome))
        {
            throw new ArgumentException("O nome é obrigatório.");
        }

        var categoriaExistente = await _repository.ObterPorIdAsync(categoria.Id);

        if (categoriaExistente is null)
        {
            throw new KeyNotFoundException("Categoria não encontrada.");
        }

        categoriaExistente.Nome = categoria.Nome;

        await _repository.AtualizarAsync(categoriaExistente);
    }
     public async Task RemoverAsync (int id)
    {
        var categoria = await _repository.ObterPorIdAsync(id);

        if (categoria is null)
        {
            throw new KeyNotFoundException("Categoria não encontrada");
        }

        await _repository.RemoverAsync(categoria);
    }


}
