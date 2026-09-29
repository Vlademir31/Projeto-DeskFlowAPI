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

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object,
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

        [Fact]
        public async Task LoginAsync_DeveRetornarTokenQuandoCredenciaisForemValidas()
        {
            var request = new LoginRequest
            {
                UserName = "Teste",
                Password = "DeskFlow@123"
            };

            var usuario = new ApplicationUser
            {
                Id = "usuario-id",
                UserName = request.UserName,
                Email = "teste@deskflow.com"
            };

            const string tokenEsperado = "token-jwt-teste";

            _userManagerMock.Setup(userManager => userManager.FindByNameAsync(request.UserName)).ReturnsAsync(usuario);
            _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(usuario, request.Password)).ReturnsAsync(true);
            _jwtServiceMock.Setup(jwtService => jwtService.GenerateTokenAsync(usuario)).ReturnsAsync(tokenEsperado);

            var resultado = await _service.LoginAsync(request);

            Assert.NotNull(resultado);
            Assert.Equal(tokenEsperado, resultado.Token);

            _userManagerMock.Verify(userName => userName.FindByNameAsync(request.UserName), Times.Once);
            _userManagerMock.Verify(userName => userName.CheckPasswordAsync(usuario, request.Password), Times.Once);
            _jwtServiceMock.Verify(jwtService => jwtService.GenerateTokenAsync(usuario), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_DeveLancarExcecaoQuandoUsuarioNaoExistir()
        {
            var request = new LoginRequest
            {
                UserName = "UsuarioInexistente",
                Password = "senha123!"
            };

            _userManagerMock.Setup(userManager => userManager.FindByNameAsync(request.UserName)).ReturnsAsync((ApplicationUser?)null);

            var excecao = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(request));

            Assert.Equal("Usuário ou senha inválidos.", excecao.Message);

            _userManagerMock.Verify(userManager => userManager.FindByNameAsync(request.UserName), Times.Once);
            _userManagerMock.Verify(userManager => userManager.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
            _jwtServiceMock.Verify(jwtService => jwtService.GenerateTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_DeveLancarExcecaoQuandoSenhaForInvalida()
        {
            var request = new LoginRequest
            {
                UserName = "Teste",
                Password = "SenhaInválida"
            };

            var usuario = new ApplicationUser
            {
                Id = "usuario-id",
                UserName = request.UserName,
                Email = "teste@deskflow.com"
            };

            _userManagerMock.Setup(userManager => userManager.FindByNameAsync(request.UserName)).ReturnsAsync(usuario);
            _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(usuario, request.Password)).ReturnsAsync(false);           
            
             var excecao = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(request));

            Assert.Equal("Usuário ou senha inválidos.", excecao.Message);

            _userManagerMock.Verify(userManager => userManager.FindByNameAsync(request.UserName), Times.Once);
            _userManagerMock.Verify(userManager => userManager.CheckPasswordAsync(usuario, request.Password), Times.Once);
            _jwtServiceMock.Verify(jwtService => jwtService.GenerateTokenAsync(usuario), Times.Never);
        }




    }
}