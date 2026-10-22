// Package adapters: buscas de cotação do domínio aportes — réplica do
// AtivosRepository.cs (brapi.dev / coingecko demo). Injetado no main como
// PriceFinder; nil = cotação/ativo não encontrado, erro = 500 no handler.
package adapters

import (
	"context"
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"net/url"
	"strings"

	"github.com/coigo/finn/internal/platform/money"
)

// Ativos replica o AtivosRepository.cs: o C# recebia um HttpClient pronto;
// aqui usamos http.DefaultClient (mesmo efeito observável: um GET por
// chamada), pois main.go injeta apenas URLs e chaves.
type Ativos struct {
	b3URL     string
	b3Key     string
	cryptoURL string
	cryptoKey string
}

func NewAtivos(b3URL, b3Key, cryptoURL, cryptoKey string) *Ativos {
	return &Ativos{
		b3URL:     b3URL,
		b3Key:     b3Key,
		cryptoURL: cryptoURL,
		cryptoKey: cryptoKey,
	}
}

// BuscarPorTicker replica o BuscarPorTicker do C#: GET {B3_API_URL}/{ticker}
// com ?token=…; regularMarketPrice de results[0]. HTTP não-2xx → (nil, nil)
// (o C# devolve null — "Ativo não encontrado!" no service); erro de rede /
// JSON / propriedade ausente → erro (no C# a exceção subia → 500).
func (a *Ativos) BuscarPorTicker(ctx context.Context, ticker string) (*money.Money, error) {
	q := url.Values{}
	q.Set("token", a.b3Key)
	req, err := http.NewRequestWithContext(ctx, http.MethodGet, a.b3URL+"/"+url.PathEscape(ticker)+"?"+q.Encode(), nil)
	if err != nil {
		return nil, err
	}
	corpo, err := pedir(req)
	if err != nil {
		return nil, err
	}
	if corpo == nil {
		return nil, nil
	}
	var payload map[string]json.RawMessage
	if err := json.Unmarshal(corpo, &payload); err != nil {
		return nil, fmt.Errorf("brapi: JSON inválido: %w", err)
	}
	results, ok := payload["results"]
	if !ok {
		return nil, fmt.Errorf("brapi: resposta sem 'results'")
	}
	var lista []map[string]json.RawMessage
	if err := json.Unmarshal(results, &lista); err != nil {
		return nil, fmt.Errorf("brapi: 'results' inválido: %w", err)
	}
	if len(lista) == 0 {
		return nil, fmt.Errorf("brapi: 'results' vazio")
	}
	raw, ok := lista[0]["regularMarketPrice"]
	if !ok {
		return nil, fmt.Errorf("brapi: result sem 'regularMarketPrice'")
	}
	var preco money.Money
	if err := json.Unmarshal(raw, &preco); err != nil {
		return nil, fmt.Errorf("brapi: regularMarketPrice inválido: %w", err)
	}
	return &preco, nil
}

// BuscarCrypto replica o BuscarCrypto do C#: GET {CRYPTO_API_URL} com
// x_cg_demo_api_key, vs_currencies=brl, precision=2 e symbols={crypto};
// resposta { "<crypto em minúsculas>": { "brl": preço } }.
func (a *Ativos) BuscarCrypto(ctx context.Context, crypto string) (*money.Money, error) {
	q := url.Values{}
	q.Set("x_cg_demo_api_key", a.cryptoKey)
	q.Set("vs_currencies", "brl")
	q.Set("precision", "2")
	q.Set("symbols", crypto)
	req, err := http.NewRequestWithContext(ctx, http.MethodGet, a.cryptoURL+"?"+q.Encode(), nil)
	if err != nil {
		return nil, err
	}
	corpo, err := pedir(req)
	if err != nil {
		return nil, err
	}
	if corpo == nil {
		return nil, nil
	}
	var payload map[string]json.RawMessage
	if err := json.Unmarshal(corpo, &payload); err != nil {
		return nil, fmt.Errorf("coingecko: JSON inválido: %w", err)
	}
	moeda, ok := payload[strings.ToLower(crypto)]
	if !ok {
		return nil, fmt.Errorf("coingecko: resposta sem %q", strings.ToLower(crypto))
	}
	var precos map[string]json.RawMessage
	if err := json.Unmarshal(moeda, &precos); err != nil {
		return nil, fmt.Errorf("coingecko: objeto de %q inválido: %w", strings.ToLower(crypto), err)
	}
	raw, ok := precos["brl"]
	if !ok {
		return nil, fmt.Errorf("coingecko: %q sem 'brl'", strings.ToLower(crypto))
	}
	var preco money.Money
	if err := json.Unmarshal(raw, &preco); err != nil {
		return nil, fmt.Errorf("coingecko: 'brl' inválido: %w", err)
	}
	return &preco, nil
}

// pedir executa a requisição: corpo lido (2xx) ou nil (não-2xx — o C# caía
// no "return null"). Erros de rede/transporte viram erro normal.
func pedir(req *http.Request) ([]byte, error) {
	resp, err := http.DefaultClient.Do(req)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()
	if resp.StatusCode < 200 || resp.StatusCode >= 300 {
		return nil, nil
	}
	return io.ReadAll(resp.Body)
}
