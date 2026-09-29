
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.DTOs.Auth;
using DeskFlow.API.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace DeskFlow.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<RegisterResponse> RegistrarAsync(RegisterRequest request)
        {
            var usuario = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var resultado = await _userManager.CreateAsync(usuario, request.Password);

            if (!resultado.Succeeded)
            {
                var erros = string.Join(";", resultado.Errors.Select(erro => erro.Description));

                throw new InvalidOperationException(erros);

            }

            return new RegisterResponse
            {
                Id = usuario.Id,
                UserName = usuario.UserName ?? string.Empty,
                Email = usuario.Email ?? string.Empty
            };

        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _userManager.FindByNameAsync(request.UserName);

            if (usuario is null)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var senhaValida = await _userManager.CheckPasswordAsync(usuario, request.Password);

            if (!senhaValida)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            var token = await _jwtService.GenerateTokenAsync(usuario);

            return new LoginResponse
            {
                Token = token
            };
        }


    }
}