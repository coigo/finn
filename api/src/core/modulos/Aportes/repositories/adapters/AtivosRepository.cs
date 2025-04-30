using System.Text.Json;
using System.Web;
using Ativos.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Infra.Repositories.Adapters;

public class AtivosRepository: IAtivosRepository {
    
    private static HttpClient _httpClient;

    public AtivosRepository(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<decimal?> BuscarPorTicker (string ticker) {

        var queryParams = new Dictionary<string, string?> {
            {"token", "gPEQrXS9Si6isryggoA8MM"}
        };

        var url = QueryHelpers.AddQueryString($"https://brapi.dev/api/quote/{ticker}", queryParams);
        var result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode) {
                var stream = await result.Content.ReadAsStreamAsync();
                using var json = await JsonDocument.ParseAsync(stream);

                var data = json.RootElement.GetProperty("results")[0].GetProperty("regularMarketPrice").GetDecimal();
                return data;
            }
            return null;
        }
    public async Task<decimal?> BuscarCrypto (string crypto) {

        var queryParams = new Dictionary<string, string?> {
            {"x_cg_demo_api_key", "CG-Z1cRPeuHd6rfjiefyYYzDxMW"},
            {"vs_currencies", "brl"},
            {"precision", "2"},
            {"symbols", "BTC"},
            
        };

        string url = "https://api.coingecko.com/api/v3/simple/price";

        var aaaa = QueryHelpers.AddQueryString(url, queryParams);
        var result = await _httpClient.GetAsync(aaaa);

            if (result.IsSuccessStatusCode) {
                var stream = await result.Content.ReadAsStreamAsync();
                using var json = await JsonDocument.ParseAsync(stream);

                var data = json.RootElement.GetProperty(crypto.ToLower()).GetProperty("brl").GetDecimal();
                return data;

            }
            return null;
        }


}