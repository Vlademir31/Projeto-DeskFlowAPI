using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Services;
using Moq;

namespace DeskFlow.Tests.Services;

public class ChamadoServiceTests
{
    [Fact]
    public async Task AdicionarAsync_DeveCriarChamadoComStatusAberto()
    {
        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        var categoriaRepositoryMock = new Mock<ICategoriaRepository>();

        var categoria = new Categoria
        {
            Id = 1,
            Nome = "Suporte de TI"
        };

        categoriaRepositoryMock.Setup(repository => repository.ObterPorIdAsync(1)).ReturnsAsync(categoria);
        
        var service = new ChamadoService(chamadoRepositoryMock.Object, categoriaRepositoryMock.Object);
        var chamado = new Chamado
        {
            Titulo = "Computador não liga",
            Descricao = "O computador do setor não está ligando.",
            Prioridade = Prioridade.Alta,
            SolicitanteNome = "João Augusto",
            CategoriaId = 1
        };

        await service.AdicionarAsync(chamado);

        Assert.Equal(StatusChamado.Aberto, chamado.Status);
        Assert.NotEqual(default(DateTime), chamado.DataAbertura);
        Assert.Null(chamado.DataFechamento);
        Assert.Null(chamado.Solucao);

        chamadoRepositoryMock.Verify(repository => repository.AdicionarAsync(chamado), Times.Once);
    }
}