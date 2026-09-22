using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/chamados")]
    public class ChamadosController : ControllerBase 
    {
        private readonly IChamadoService _service;
        public  ChamadosController(IChamadoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Chamado>> Adicionar(Chamado chamado)
        {
            await _service.AdicionarAsync(chamado);

            return StatusCode(StatusCodes.Status201Created, chamado);
        }
    }
}