using DeskFlow.API.Models.DTOs.Auth;
using DeskFlow.API.Models.Identity;

namespace DeskFlow.API.Interfaces
{
    public interface IAuthService
    {
      Task<ApplicationUser> RegistrarAsync(RegisterRequest request);   
    }
}