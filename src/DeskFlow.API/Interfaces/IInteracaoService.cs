using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface IInyeracaoService
    {
        Task AdicionarAsync(int chamadoId, Interacao interacao);
    }
}