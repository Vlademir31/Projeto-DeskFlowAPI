using DeskFlow.API.Models.Enums;

namespace DeskFlow.API.Models.Entities;

public class Chamado
{
    public int id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Prioridade Prioridade { get; set; }
    public StatusChamado status { get; set; }
    public string SolicitanteNome { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; } // esse campo pode não possuir valor
    public string? Solucao {get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!; // Categoria 1 - N Chamado

}