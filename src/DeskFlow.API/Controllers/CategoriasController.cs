
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
    }
}