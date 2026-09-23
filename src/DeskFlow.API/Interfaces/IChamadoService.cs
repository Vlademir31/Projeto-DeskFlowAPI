using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Interfaces;
 public interface IChamadoService
{
    Task<List<Chamado>> ObterTodosAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    Task<Chamado?> ObterPorIdAsync(int id);
    Task AdicionarAsync(Chamado chamado);
    Task IniciarAsync (int id);
    Task EncerrarAsync(int id, string solucao);
}