using DeskFlow.API.Interfaces;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.DTOs;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Chamado>> ObterPorId(int id)
        {
            var chamado = await _service.ObterPorIdAsync(id);

            if (chamado is null)
            {
                return NotFound();
            }

            return Ok(chamado);
        }

        [HttpPost]
        public async Task<ActionResult<Chamado>> Adicionar(Chamado chamado)
        {
            await _service.AdicionarAsync(chamado);

            return StatusCode(StatusCodes.Status201Created, chamado);
        }

        [HttpPatch("{id:int}/iniciar")]
        public async Task<IActionResult> Iniciar (int id )
        {
            await _service.IniciarAsync(id);

            return NoContent();
        }

        [HttpPatch("{id:int}/encerrar")]
        public async Task<IActionResult> Encerrar (int id, [FromBody] EncerrarChamadoRequest request)
        {
            await _service.EncerrarAsync(id, request.Solucao);

            return NoContent();
        }
    }
}