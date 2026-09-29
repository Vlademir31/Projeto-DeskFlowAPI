using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeskFlow.API.Services;
using DeskFlow.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DeskFlow.Tests.Services
{
    public class JwtServiceTests
    {
      private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
      private readonly Mock<IConfiguration> _configurationMock;
      private readonly JwtService _service;
      public JwtServiceTests ()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

             _userManagerMock = new Mock<UserManager<ApplicationUser>>( userStoreMock.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

            _configurationMock = new  Mock<IConfiguration>(); _service = new JwtService(
            _configurationMock.Object, _userManagerMock.Object);
          
        } 

        [Fact]
        public async Task GenerateTokenAsync_DeveGerarTokenComSucesso()
        {
            var usuario = new ApplicationUser
            {
                Id = "usuario-123",
                UserName = "Teste",
                Email = "teste@deskflow.com"
            };

            _configurationMock.Setup(configuration => configuration["Jwt:Key"]).Returns("ChaveSuperSecretaParaTestesDoDeskFlow123456789");
            _configurationMock.Setup(configuration => configuration["Jwt:Issuer"]).Returns("DeskFlow.API");
            _configurationMock.Setup(configuration => configuration["Jwt:Audience"]).Returns("DeskFlow.Client");
            _configurationMock.Setup(configuration => configuration.GetSection("Jwt:ExpirationInMinutes")).Returns(MockSection("60"));
            _userManagerMock.Setup(userManager => userManager.GetRolesAsync(usuario)).ReturnsAsync(new List<string>());

            var token = await _service.GenerateTokenAsync(usuario);

            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.Equal("usuario-123", jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            Assert.Equal("teste@deskflow.com",jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
            Assert.NotEmpty(jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        } 

          [Fact]
        public async Task GenerateTokenAsync_DeveAdicionarRolesComoClaims()
        {
            var usuario = new ApplicationUser
            {
                Id = "usuario-123",
                UserName = "Teste",
                Email = "teste@deskflow.com"
            };

            _configurationMock.Setup(configuration => configuration["Jwt:Key"]).Returns("ChaveSuperSecretaParaTestesDoDeskFlow123456789");
            _configurationMock.Setup(configuration => configuration["Jwt:Issuer"]).Returns("DeskFlow.API");
            _configurationMock.Setup(configuration => configuration["Jwt:Audience"]).Returns("DeskFlow.Client");
            _configurationMock.Setup(configuration => configuration.GetSection("Jwt:ExpirationInMinutes")).Returns(MockSection("60"));
            _userManagerMock.Setup(userManager => userManager.GetRolesAsync(usuario)).ReturnsAsync(new List<string>{ "Administrador"});
        
           var token = await _service.GenerateTokenAsync(usuario);

           var handler = new JwtSecurityTokenHandler();
           var jwt = handler.ReadJwtToken(token);

           var roleClaim = jwt.Claims.First(claim => claim.Type == ClaimTypes.Role);

           Assert.Equal("Administrador", roleClaim.Value);
        }

        [Fact]
        public async Task GenerateTokenAsync_DeveLancarExcecaoQuandoChaveNaoEstiverConfigurada()
        {
            var usuario = new ApplicationUser
            {
                Id = "usuario-123",
                Email = "teste@deskflow.com"
            };

            _configurationMock.Setup(configuration => configuration["Jwt:Key"]).Returns((string?)null);

            var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GenerateTokenAsync(usuario));

            Assert.Equal("A chave JWT não está configurada.", excecao.Message);
        }

        private static IConfigurationSection MockSection(string value)
        {
            var sectionMock = new Mock<IConfigurationSection>();

            sectionMock.Setup(section => section.Value).Returns(value);

            return sectionMock.Object;
        }

      


    }
}