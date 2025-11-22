using Aportes.UseCases;
using Infra.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Infra.Http.Controllers.Aportes;

[ApiController]
[Route("api/aportes")]
public class BuscarCategorias
{

    private readonly BuscarCategoriasUseCase _useCase;

    public BuscarCategorias(BuscarCategoriasUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet]
    [Route("categorias")]
    public List<SelectValues> Handle()
    {
        return this._useCase.Execute(Unity.Value);
    }

}