
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> ObterTodosAsync();

        Task<Categoria?> ObterPorIdAsync(int id);

        Task AdicionarAsync(Categoria categoria);

        Task AtualizarAsync(Categoria categoria);

        Task RemoverAsync(Categoria categoria);
    }
}