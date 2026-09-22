using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces;
 public interface IChamadoService
{
 
    Task AdicionarAsync(Chamado chamado);
    Task IniciarAsync (int id);
    Task EncerrarAsync(int id, string solucao);
}