using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;
using Moq;

namespace DeskFlow.Tests.Services;
public class CategoriaServiceTests
{
    [Fact]
    public async Task AdicionarAsync_DeveAdicionarCategoriaValida()
    {
       var repositoryMock = new Mock<ICategoriaRepository>();

       var service = new CategoriaService(repositoryMock.Object);

       var categoria = new Categoria
       {
           Nome = "Suporte de TI"
       };

       await service.AdicionarAsync(categoria);

       repositoryMock.Verify(repository => repository.AdicionarAsync(categoria), Times.Once);
     
    }

    [Fact]
    public async Task AdicionarAsync_DeveLancarExcecaoQuandoNomeForVazio()
    {
      var repositoryMock = new Mock<ICategoriaRepository>();

      var service = new CategoriaService(repositoryMock.Object);

      var categoria = new Categoria
      {
          Nome = String.Empty
      };

      var excecao = await Assert.ThrowsAsync<ArgumentException>(() => service.AdicionarAsync(categoria));

      Assert.Equal("O nome é obrigatório.", excecao.Message);

      repositoryMock.Verify( repository => repository.AdicionarAsync(It.IsAny<Categoria>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarAsync_DeveLancarExcecaoQuandoCategoriaNaoExistir()
    {
       var repositoryMock = new Mock<ICategoriaRepository>();

       repositoryMock.Setup(repository => repository.ObterPorIdAsync(999)).ReturnsAsync((Categoria?)null);

       var service = new CategoriaService(repositoryMock.Object);

       var Categoria = new Categoria
       {
           Id = 999,
           Nome = "Categoria inexistente"
       };

       var excecao = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.AtualizarAsync(Categoria));

       Assert.Equal("Categoria não encontrada.", excecao.Message);

       repositoryMock.Verify(repoistory => repoistory.AtualizarAsync(It.IsAny<Categoria>()),
       Times.Never);
    }

    [Fact]
    public async Task RemoverAsync_DeveLancarExcecaoQuandoCategoriaPossuirChamados()
    {
     var repositoryMock = new Mock<ICategoriaRepository>();

     var categoria = new Categoria
     {
         Id = 4,
         Nome = "Suporte de TI"
     };

     repositoryMock.Setup(repository => repository.ObterPorIdAsync(4)).ReturnsAsync(categoria);

     repositoryMock.Setup(repository => repository.PossuiChamadosAsync(4)).ReturnsAsync(true);

     var service = new CategoriaService(repositoryMock.Object);

     var excecao = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.RemoverAsync(4));

     Assert.Equal("Não é possível excluir uma categoria que possui chamados.", excecao.Message);

     repositoryMock.Verify(repository => repository.RemoverAsync(It.IsAny<Categoria>()), Times.Never);
    }

    [Fact]
    public async Task RemoverAsync_DeveLancarExcecaoQuandoCategoriaNaoExistir()
    {
      var repositoryMock = new Mock<ICategoriaRepository>();

      repositoryMock.Setup(repositoy => repositoy.ObterPorIdAsync(9898)).ReturnsAsync((Categoria?)null);

      var service = new CategoriaService(repositoryMock.Object);

      var excecao = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.RemoverAsync(9898));

      Assert.Equal("Categoria não encontrada", excecao.Message);

      repositoryMock.Verify(repository => repository.PossuiChamadosAsync(It.IsAny<int>()), Times.Never);

      repositoryMock.Verify(repository => repository.RemoverAsync(It.IsAny<Categoria>()), Times.Never);
    }
   
}