using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repository;

namespace DeskFlow.API.Services;

    public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
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

}
