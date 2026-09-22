
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Interfaces
{
    public interface InteracaoRepository
    {
     Task AdicionarAsync (Interacao interacao);  
    }
}