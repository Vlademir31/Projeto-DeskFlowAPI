
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface IInteracaoRepository
    {
     Task AdicionarAsync (Interacao interacao);  
    }
}