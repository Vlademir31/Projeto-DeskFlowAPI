
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface IChamadoRepository
    {
       Task AdicionarAsync (Chamado chamado); 
    }
}