
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Services
{
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _repository;
        private readonly ICategoriaRepository _categoriarepository;
        public ChamadoService(IChamadoRepository repository, ICategoriaRepository categoriaRepository)
        {
            _repository = repository;
            _categoriarepository = categoriaRepository;
        }
        public async Task<List<Chamado>> ObterTodosAsync(StatusChamado? status, Prioridade? prioridade, int? categoriaId)
        {
            return await _repository.ObterTodosAsync(status, prioridade, categoriaId);
        }
        public async Task<Chamado?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id);
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

            var categoria = await _categoriarepository.ObterPorIdAsync(chamado.CategoriaId);

            if (categoria is null)
            {
                throw new KeyNotFoundException ("Categoria não encontrada.");
            }

            chamado.Status = StatusChamado.Aberto;
            chamado.DataAbertura = DateTime.Now;
            chamado.DataFechamento = null;
            chamado.Solucao = null;

            await _repository.AdicionarAsync(chamado);
        }
        public async Task IniciarAsync(int id)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado is null)
            {
                throw new KeyNotFoundException("Chamado não encontrado.");
            }

            if (chamado.Status != StatusChamado.Aberto)
            {
                throw new InvalidOperationException("Somente chamados com status Aberto podem ser iniciados.");
            }

            chamado.Status = StatusChamado.EmAndamento;

            await _repository.AtualizarAsync(chamado);
        }
        public async Task EncerrarAsync (int id, string solucao)
        {
            var chamado = await _repository.ObterPorIdAsync(id);

            if (chamado is null)
            {
                throw new KeyNotFoundException("Chamado não encontrado.");
            }

            if (string.IsNullOrWhiteSpace(solucao))
            {
                throw new ArgumentException("Solução obrigatória.");
            }

            if (chamado.Status == StatusChamado.Fechado)
            {
                throw new InvalidOperationException("Chamado já está fechado.");
            }

            chamado.Status = StatusChamado.Fechado;
            chamado.Solucao = solucao;
            chamado.DataFechamento = DateTime.Now;

            await _repository.AtualizarAsync(chamado);
        }
    }
}