
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.DTOs.Auth;
using DeskFlow.API.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace DeskFlow.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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


    }
}