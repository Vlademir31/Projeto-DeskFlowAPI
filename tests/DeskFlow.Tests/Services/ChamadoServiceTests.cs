using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Enums;
using DeskFlow.API.Repository;
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

    [Fact]
    public async Task IniciarAsync_DeveAlterarChamadoParaEmAndamento()
    {
        var chamadoRepositoryMock = new Mock<IChamadoRepository>();

        var categoriaRepositoryMock = new Mock<ICategoriaRepository>();

        var chamado = new Chamado
        {
            Id = 1,
            Titulo = "Computador não liga",
            Descricao = "O computador do setor não está ligando.",
            Prioridade = Prioridade.Alta,
            SolicitanteNome = "João Augusto",
            CategoriaId = 1
        };

        chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(1)).ReturnsAsync(chamado);

        var service = new ChamadoService(chamadoRepositoryMock.Object, categoriaRepositoryMock.Object);

        await service.IniciarAsync(1);

        Assert.Equal(StatusChamado.EmAndamento, chamado.Status);

        chamadoRepositoryMock.Verify(repository => repository.AtualizarAsync(chamado), Times.Once);

    }

     [Fact]

        public async Task IniciarAsync_DeveLancarExcecaoQuandoChamadoNaoEstiverAberto()
        {
         var  chamadoRepositoryMock = new Mock<IChamadoRepository>();

         var categoriaRepositoryMock = new Mock<ICategoriaRepository>();

         var chamado = new Chamado
         {
             Id = 2,
             Titulo= "Computador não liga",
             Descricao = "O Computador do setor não está ligando.",
             Prioridade = Prioridade.Alta,
             Status = StatusChamado.EmAndamento,
             SolicitanteNome = "João Augusto",
             CategoriaId = 1
         };

         chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(2)).ReturnsAsync(chamado);

         var service = new ChamadoService(chamadoRepositoryMock.Object, categoriaRepositoryMock.Object);

         var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => service.IniciarAsync(2));

         Assert.Equal("Somente chamados com status Aberto podem ser iniciados.", excecao.Message);

         chamadoRepositoryMock.Verify(repository => repository.AtualizarAsync(It.IsAny<Chamado>()), Times.Never);
        }

     [Fact]
     public async Task EncerrarAsync_DeveFecharChamadoComSolucao()
    {
       var chamadoRepositoryMock = new Mock<IChamadoRepository>();

       var categoriaRepositoryMock = new Mock<ICategoriaRepository>();

       var chamado = new Chamado
       {
           Id = 3,
           Titulo = "Computador não liga",
           Descricao = "O computador do setor não está ligando.",
           Prioridade = Prioridade.Alta,
           Status = StatusChamado.EmAndamento,
           SolicitanteNome = "João Augusto",
           CategoriaId = 1
       };

       chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(3)).ReturnsAsync(chamado);

       var service = new ChamadoService(chamadoRepositoryMock.Object, categoriaRepositoryMock.Object);

       await service.EncerrarAsync(3, "Foi realizada a troca da fonte de alimentação.");

       Assert.Equal(StatusChamado.Fechado, chamado.Status);
       Assert.Equal("Foi realizada a troca da fonte de alimentação.", chamado.Solucao);
       Assert.NotNull(chamado.DataFechamento);

       chamadoRepositoryMock.Verify(repository => repository.AtualizarAsync(chamado), Times.Once);
    }

    [Fact]
    public async Task EcerrarAsync_DeveLancarExcecaoQuandoSolucaoForVazia()
    {
      var chamadoRepositoryMock = new Mock<IChamadoRepository>();

      var categoriaRepositoryMock = new Mock<ICategoriaRepository>();

      var chamado = new Chamado
      {
          Id = 4,
          Titulo = "Computador não liga",
          Descricao = "O computador do setor não está ligando.",
          Prioridade = Prioridade.Alta,
          Status = StatusChamado.EmAndamento,
          SolicitanteNome = "João Augusto",
          CategoriaId = 1
      };

      chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(4)).ReturnsAsync(chamado);

      var service = new ChamadoService(chamadoRepositoryMock.Object, categoriaRepositoryMock.Object);

      var excecao = await Assert.ThrowsAsync<ArgumentException>(() => service.EncerrarAsync(4, string.Empty));

      Assert.Equal("Solução obrigatória.", excecao.Message);

      Assert.Equal(StatusChamado.EmAndamento, chamado.Status);
      Assert.Null(chamado.Solucao);
      Assert.Null(chamado.DataFechamento);

      chamadoRepositoryMock.Verify(repository => repository.AtualizarAsync(It.IsAny<Chamado>()), Times.Never); 
    }

     [Fact]
    public async Task EcerrarAsync_DeveLancarExcecaoQuandoChamamdoJaEstiverFechado()
    {
      var chamadoRepositoryMock = new Mock<IChamadoRepository>();

      var categoriaRepositoryMock = new Mock<ICategoriaRepository>();

      var chamado = new Chamado
      {
          Id = 5,
          Titulo = "Computador não liga",
          Descricao = "O computador do setor não está ligando.",
          Prioridade = Prioridade.Alta,
          Status = StatusChamado.Fechado,
          SolicitanteNome = "João Augusto",
          CategoriaId = 1,
          Solucao = "Fonte substituida.",
          DataFechamento = DateTime.Now
      };

      chamadoRepositoryMock.Setup(repository => repository.ObterPorIdAsync(5)).ReturnsAsync(chamado);

      var service = new ChamadoService(chamadoRepositoryMock.Object, categoriaRepositoryMock.Object);

      var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => service.EncerrarAsync(5, "Tentativa de encerramento novamente."));

      Assert.Equal("Chamado já está fechado.", excecao.Message);

      Assert.Equal(StatusChamado.Fechado, chamado.Status);
      Assert.Equal("Fonte substituida.", chamado.Solucao);
     

      chamadoRepositoryMock.Verify(repository => repository.AtualizarAsync(It.IsAny<Chamado>()), Times.Never); 
    }
}   