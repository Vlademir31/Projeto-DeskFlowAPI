using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.DTOs.Auth;
using DeskFlow.API.Models.Identity;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DeskFlow.Tests.Services
{
    public class AuthServiceTests
    {
     private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
     private readonly Mock<IJwtService> _jwtServiceMock;
     private readonly AuthService _service;
     
     public AuthServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
             
            _userManagerMock = new Mock<UserManager<ApplicationUser>>( userStoreMock.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

            _jwtServiceMock = new Mock<IJwtService>();
            _service = new AuthService(_userManagerMock.Object, _jwtServiceMock.Object);
        } 

        [Fact]
        public async Task RegistrarAsync_DeveRegistrarUsuarioComSucesso()
        {
            var request = new RegisterRequest
            {
                UserName = "Teste",
                Email = "teste@deskflow.com",
                Password = "DeskFlow@123"
            };
            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(),
            request.Password)).ReturnsAsync(IdentityResult.Success);

            var resultado = await _service.RegistrarAsync(request);

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado.Id);
            Assert.Equal(request.UserName, resultado.UserName);
            Assert.Equal(request.Email, resultado.Email);

            _userManagerMock.Verify(userManager => userManager.CreateAsync(It.Is<ApplicationUser>(usuario =>
            usuario.UserName == request.UserName && usuario.Email == request.Email), request.Password), Times.Once);
        }  

        [Fact]
        public async Task RegistrarAsync_DeveLancarExcecaoQuandoIdentityRecusarCadastro()
        {
            var request = new RegisterRequest
            {
                UserName = "Teste",
                Email = "Teste@deskflow.com",
                Password = "senha-invalida"
            };

            var erros = new IdentityError
            {

                Description = "A senha é muito curta."
            };
            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(),
            request.Password)).ReturnsAsync(IdentityResult.Failed(erros));

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RegistrarAsync(request));

            Assert.Contains("A senha é muito curta.", excecao.Message);

            _userManagerMock.Verify(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(),
            request.Password), Times.Once);
        }

        
    }
}