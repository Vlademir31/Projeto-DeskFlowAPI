
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
      private readonly CategoriaService _service;
      public CategoriasController (CategoriaService service)
        {
            _service = service;
        }  

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> ObterTodos()
        {
            var categorias = await _service.ObterTodosAsync();

            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categoria>> ObterPorId (int id)
        {
            var categoria = await _service.ObterPorIdAsync(id);

            if (categoria is null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }
    }
}