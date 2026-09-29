using DeskFlow.API.Models.DTOs.Auth;

namespace DeskFlow.API.Interfaces
{
    public interface IAuthService
    {
      Task<RegisterResponse> RegistrarAsync(RegisterRequest request);

      Task<LoginResponse> LoginAsync(LoginRequest request);   
    }
}