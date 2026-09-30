using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Services;

public class InteracaoService : IInteracaoService
{
    private readonly IInteracaoRepository _repository;
    private readonly IChamadoRepository _chamadoRepository;
    public InteracaoService (IInteracaoRepository repository, IChamadoRepository chamadoRepository)
    {
        _repository = repository;
        _chamadoRepository = chamadoRepository;
    }
    public async Task AdicionarAsync (int chamadoId, Interacao interacao)
    {
        if (chamadoId <= 0)
        {
            throw new ArgumentException ("Id do chamado obrigatório.");
        }

        if  (string.IsNullOrWhiteSpace(interacao.Autor))
        {
            throw new ArgumentException("Autor é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(interacao.Mensagem))
        {
            throw new ArgumentException("Mensagem obrigatória.");
        }

        var chamado = await _chamadoRepository.ObterPorIdAsync(chamadoId);

        if ( chamado is null)
        {
            throw new KeyNotFoundException("Chamado não encontrado.");
        }

        if (chamado.Status == StatusChamado.Fechado)
        {
            throw new InvalidOperationException("Não é possível adicionar uma interação a um chamado fechado.");
        }

        interacao.ChamadoId = chamadoId;
        interacao.DataRegistro = DateTime.Now;

        await _repository.AdicionarAsync(interacao);
    }
}