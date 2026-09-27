
using DeskFlow.API.Models.Identity;

namespace DeskFlow.API.Interfaces
{
    public interface IJwtService
    {
      Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}