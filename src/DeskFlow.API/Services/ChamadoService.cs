

using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _repository;
        public ChamadoService(IChamadoRepository repository)
        {
            _repository = repository;
        }
        public async Task AdicionarAsync(Chamado chamado)
        {
            if  (string.IsNullOrWhiteSpace(chamado.Titulo))
            {
                throw new ArgumentException ("Título obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(chamado.Descricao))
            {
                throw new ArgumentException("Descrição obrigatória.");
            }

            if (string.IsNullOrWhiteSpace(chamado.SolicitanteNome))
            {
                throw new ArgumentException("Nome do solicitante obrigatório.");
            }

            if (chamado.CategoriaId <= 0)
            {
                throw new ArgumentException("Categoria obrigatório.");
            }

            chamado.Status = StatusChamado.Aberto;
            chamado.DataAbertura = DateTime.Now;
            chamado.DataFechamento = null;
            chamado.Solucao = null;

            await _repository.AdicionarAsync(chamado);
        }
    }
}