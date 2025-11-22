using Aportes.DTOS;
using Aportes.Models;

namespace Infra.Repositories;

public interface IAporteHistoricoRepository {

    public Task<List<AporteHistorico>> Buscar (HistoricoFiltrosDTO filtros);
    
    public Task<AporteHistorico> CriarRegistro (AporteHistorico data);
    
}