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
            {"token", Environment.GetEnvironmentVariable("B3_API_KEY")}
        };
        var url = QueryHelpers.AddQueryString($"{Environment.GetEnvironmentVariable("B3_API_URL")}/{ticker}", queryParams);
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
            {"x_cg_demo_api_key", Environment.GetEnvironmentVariable("CRYPTO_API_KEY")},
            {"vs_currencies", "brl"},
            {"precision", "2"},
            {"symbols", "BTC"},
            
        };

        Console.WriteLine($"{Environment.GetEnvironmentVariable("CRYPTO_API_URL")}/{crypto}");


        var url = QueryHelpers.AddQueryString($"{Environment.GetEnvironmentVariable("CRYPTO_API_URL")}", queryParams);
        var result = await _httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode) {
                var stream = await result.Content.ReadAsStreamAsync();
                using var json = await JsonDocument.ParseAsync(stream);

                var data = json.RootElement.GetProperty(crypto.ToLower()).GetProperty("brl").GetDecimal();
                return data;

            }
            return null;
        }


}