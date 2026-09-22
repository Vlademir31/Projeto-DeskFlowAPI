using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces;
 public interface IChamadoService
{
    Task AdicionarAsync(Chamado chamado);
}