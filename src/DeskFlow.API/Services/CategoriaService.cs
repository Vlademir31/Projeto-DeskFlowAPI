using DeskFlow.API.Repository;

namespace DeskFlow.API.Services;

    public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
    {
        _repository = repository;
    }

}
