using Aportes.Models;
using Infra.Shared;

namespace Aportes.UseCases;

public class BuscarCategoriasUseCase : IUseCaseSyncronous<Unity, List<SelectValues>>
{

    public List<SelectValues> Execute(Unity _)
    {
        var categorias = Enum.GetValues(typeof(AporteCategoria))
            .Cast<AporteCategoria>().Select(e => new SelectValues((int)e, e.ToString()))
            .ToList();
        return categorias;
    }

}