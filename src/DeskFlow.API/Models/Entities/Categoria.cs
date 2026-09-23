using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeskFlow.API.Models.Entities;
public class Categoria
{
  public int Id { get; set; }
  
  [Required]
  [MaxLength(100)]
  public string Nome { get; set; } = string.Empty;  

  [JsonIgnore]
  public ICollection<Chamado> Chamados { get; set; } = []; // um categoria pode estar relacionada a zero, um ou vários chamados.
}