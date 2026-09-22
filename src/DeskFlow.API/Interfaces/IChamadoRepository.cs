
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface IChamadoRepository
    {
        Task<Chamado?> ObterPorIdAsync(int id);
       Task AdicionarAsync (Chamado chamado); 
    }
}