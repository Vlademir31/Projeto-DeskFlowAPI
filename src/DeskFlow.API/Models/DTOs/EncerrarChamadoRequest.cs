
using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.DTOs
{
    public class EncerrarChamadoRequest
    {
        [Required]
        public string Solucao { get; set; } = string.Empty;
    }
}