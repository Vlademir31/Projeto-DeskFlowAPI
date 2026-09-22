using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface IInteracaoService
    {
        Task AdicionarAsync(int chamadoId, Interacao interacao);
    }
}