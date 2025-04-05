using System.Web;
using Ativos.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Infra.Repositories.Adapters;

public class AtivosRepository: IAtivosRepository {
    
    private readonly HttpClient _client = new () {
        BaseAddress =  new Uri("http://brapi.dev/api/")
    };

    public async Task<Ativo> BuscarPorTicker (string ticker) {

        var queryParams = new Dictionary<string, string> {
            {"token", "gPEQrXS9Si6isryggoA8MM"}
        };

        var url = QueryHelpers.AddQueryString($"quote/{ticker}", queryParams);
        var result = await _client.GetAsync(url);

            var parsed = await result.Content.ReadFromJsonAsync<AtivoResponse>();
            return parsed.Results[0];

        }
}