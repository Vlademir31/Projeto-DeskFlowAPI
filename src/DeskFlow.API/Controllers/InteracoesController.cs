
using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/chamados/{chamadoId:int}/inetracoes")]
    public class InteracoesController : ControllerBase
    {
        private readonly IInteracaoService _service;
        public InteracoesController (IInteracaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Interacao>> AdicionarAsync(int chamadoId, Interacao interacao)
        {
         await _service.AdicionarAsync(chamadoId, interacao);

         return StatusCode (StatusCodes.Status201Created, interacao);
        }
    }

}