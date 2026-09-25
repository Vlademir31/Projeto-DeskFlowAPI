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
   
}