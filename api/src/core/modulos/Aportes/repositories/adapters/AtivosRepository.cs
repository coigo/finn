using System.Web;
using Ativos.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Infra.Repositories.Adapters;

public class AtivosRepository: IAtivosRepository {
    
    private static HttpClient _httpClient;

    public AtivosRepository(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<Ativo> BuscarPorTicker (string ticker) {

        var queryParams = new Dictionary<string, string> {
            {"token", "gPEQrXS9Si6isryggoA8MM"}
        };

        var url = QueryHelpers.AddQueryString($"quote/{ticker}", queryParams);
        var result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode) {
                var parsed = await result.Content.ReadFromJsonAsync<AtivoResponse>();
                return parsed.Results[0];
            }
            return new Ativo();
        }


}