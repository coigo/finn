namespace Ativos.Models;

using System;
using System.Text.Json.Serialization;

public class AtivoResponse
{
    [JsonPropertyName("results")]
    public List<Ativo> Results { get; set; }

    [JsonPropertyName("requestedAt")]
    public DateTime RequestedAt { get; set; }

    [JsonPropertyName("took")]
    public string Took { get; set; }
}

public class Ativo
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("marketCap")]
    public decimal? MarketCap { get; set; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }

    [JsonPropertyName("longName")]
    public string LongName { get; set; }

    [JsonPropertyName("regularMarketChange")]
    public decimal? RegularMarketChange { get; set; }

    [JsonPropertyName("regularMarketChangePercent")]
    public decimal? RegularMarketChangePercent { get; set; }

    [JsonPropertyName("regularMarketTime")]
    public DateTime? RegularMarketTime { get; set; }

    [JsonPropertyName("regularMarketPrice")]
    public decimal? RegularMarketPrice { get; set; }

    [JsonPropertyName("regularMarketDayHigh")]
    public decimal? RegularMarketDayHigh { get; set; }

    [JsonPropertyName("regularMarketDayRange")]
    public string RegularMarketDayRange { get; set; }

    [JsonPropertyName("regularMarketDayLow")]
    public decimal? RegularMarketDayLow { get; set; }

    [JsonPropertyName("regularMarketVolume")]
    public int? RegularMarketVolume { get; set; }

    [JsonPropertyName("regularMarketPreviousClose")]
    public decimal? RegularMarketPreviousClose { get; set; }

    [JsonPropertyName("regularMarketOpen")]
    public decimal? RegularMarketOpen { get; set; }

    [JsonPropertyName("fiftyTwoWeekRange")]
    public string FiftyTwoWeekRange { get; set; }

    [JsonPropertyName("fiftyTwoWeekLow")]
    public decimal? FiftyTwoWeekLow { get; set; }

    [JsonPropertyName("fiftyTwoWeekHigh")]
    public decimal? FiftyTwoWeekHigh { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("priceEarnings")]
    public decimal? PriceEarnings { get; set; }

    [JsonPropertyName("earningsPerShare")]
    public decimal? EarningsPerShare { get; set; }

    [JsonPropertyName("logourl")]
    public string LogoUrl { get; set; }
}