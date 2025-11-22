using Aportes.DTOS;
using Aportes.Models;

namespace Infra.Repositories;

public interface IAporteRepository {
    public Task<List<Aporte>> BuscarTodos ();
    
    public Task<List<AporteHistoricoDTO>> BuscarDetalhesDoAporte (string Identificador);

    public Task<Aporte?> BuscarPorIdentificador (string Identificador);

    public Task<List<Aporte>> BuscarPorCategoria (AporteCategoria categoria);
    
    public Task<Aporte> CriarAporte (Aporte data);

    public Task<AtualizarAporteDTO> AtualizarAporte (int id, AtualizarAporteDTO data);

}