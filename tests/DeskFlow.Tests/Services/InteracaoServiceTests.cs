using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Services;
using Moq;

namespace DeskFlow.Tests.Services;

public class InteracaoServiceTests
{
    [Fact]
    public async Task AdicionarAsync_DeveAdicionarInteracaoParaChamadoAberto()
    {
        var interacaoRepositoryMock = new Mock<IInteracaoRepository>();

        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        var chamado = new Chamado
        {
            Id = 1,
            Status = StatusChamado.Aberto
        };

        chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(1)).ReturnsAsync(chamado);

        var service = new InteracaoService(interacaoRepositoryMock.Object, chamadoRepositoryMock.Object);
        var interacao = new Interacao
        {
            Autor = "Equipe de suporte",
            Mensagem = "Atendimento iniciado"

        };

        await service.AdicionarAsync(1, interacao);

        Assert.Equal(1, interacao.ChamadoId);
        Assert.NotEqual(default, interacao.DataRegistro);

        interacaoRepositoryMock.Verify(repository => repository.AdicionarAsync(interacao), Times.Once);
    }

    [Fact]
    public async Task AdicionarAsync_DeveLancarExcecaoQuandoChamadoNaoExistir()
    {
        var interacaoRepositoryMock = new Mock<IInteracaoRepository>();

        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(9898)).ReturnsAsync((Chamado?)null);

        var service = new InteracaoService(interacaoRepositoryMock.Object, chamadoRepositoryMock.Object);

        var interacao = new Interacao
        {
            Autor = "Equipe de suporte",
            Mensagem = "Tentativa de interacao."

        };

        var excecao = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AdicionarAsync(9898, interacao));

        Assert.Equal("Chamado não encontrado.", excecao.Message);

        interacaoRepositoryMock.Verify(repository => repository.AdicionarAsync(It.IsAny<Interacao>()), Times.Never);

    }

    [Fact]

    public async Task AdicionarAsync_DeveLancarExcecaoQuandoChamadoEstiverFechado()
    {
        var interacaoRepositoryMock = new Mock<IInteracaoRepository>();

        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        var chamado = new Chamado
        {
            Id = 2,
            Status = StatusChamado.Fechado
        };

        chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(2)).ReturnsAsync(chamado);

        var service = new InteracaoService(interacaoRepositoryMock.Object, chamadoRepositoryMock.Object);

        var interacao = new Interacao
        {
            Autor = "Equipe de suporte",
            Mensagem = "Nova mensagem."

        };
        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => service.AdicionarAsync(2, interacao));

        Assert.Equal("Não é possível adicionar uma interação a um chamado fechado.", excecao.Message);
        
        chamadoRepositoryMock.Verify(repository => repository.ObterPorIdAsync(It.IsAny<int>()), Times.Once);

        interacaoRepositoryMock.Verify(repository => repository.AdicionarAsync(It.IsAny<Interacao>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarAsync_DeveLancarExcecaoQuandoAutorForVazio()

    {
        var interacaoRepositoryMock = new Mock<IInteracaoRepository>();

        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        var service = new InteracaoService(interacaoRepositoryMock.Object, chamadoRepositoryMock.Object);

        var interacao = new Interacao
        {
            Autor = string.Empty,
            Mensagem = "Mensagem válida."
        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => service.AdicionarAsync(1, interacao));

        Assert.Equal("Autor é obrigatório.", excecao.Message);
        
        chamadoRepositoryMock.Verify(repository => repository.ObterPorIdAsync(It.IsAny<int>()), Times.Never);

        interacaoRepositoryMock.Verify(repository => repository.AdicionarAsync(It.IsAny<Interacao>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarAsync_DeveLancarExcecaoQuandoMensagemForVazia()
    {
        var interacaoRepositoryMock = new Mock<IInteracaoRepository>();

        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        var service = new InteracaoService(interacaoRepositoryMock.Object, chamadoRepositoryMock.Object);

         var interacao = new Interacao
        {
            Autor = "Equipe de suporte",
            Mensagem = String.Empty

        };

        var excecao = await Assert.ThrowsAsync<ArgumentException>(() => service.AdicionarAsync(1, interacao));

        Assert.Equal("Mensagem obrigatória.", excecao.Message);
        
        chamadoRepositoryMock.Verify(repository => repository.ObterPorIdAsync(It.IsAny<int>()), Times.Never);

        interacaoRepositoryMock.Verify(repository => repository.AdicionarAsync(It.IsAny<Interacao>()), Times.Never);
    }

}