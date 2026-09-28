using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
       private readonly IAuthService _authService;
       public AuthController(IAuthService authService)
        {
            _authService = authService;
        } 

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Registrar(RegisterRequest request)
        {
            var usuario = await _authService.RegistrarAsync(request);

            return StatusCode(StatusCodes.Status201Created, usuario);
        }
    }
}