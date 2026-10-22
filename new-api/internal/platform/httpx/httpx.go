// Package httpx centraliza a escrita de respostas JSON e a tradução de erros
// de domínio (sentinels verificados com errors.Is) em status/HTTP no formato
// fixo {"status": <código>, "message": "<texto>"} — mesmo shape do ErrorHandler
// do C#.
//
// Contrato acordado (B1):
//   - erros de domínio (sentinels de ErrNotFound/ErrBusiness) → 400;
//   - erro inesperado → 500 vazando err.Error() (o ErrorHandler do C#
//     serializa ex.Message no 500; não usa texto fixo).
package httpx

import (
	"encoding/json"
	"fmt"
	"log/slog"
	"net/http"
	"strconv"
	"strings"
	"time"
)

// ParseDate aceita os formatos que o front envia (YYYY-MM-DD e RFC3339);
// divergência documentada: o C# aceitava mais formatos via DateTime.Parse.
func ParseDate(s string) (time.Time, error) {
	for _, layout := range []string{"2006-01-02", time.RFC3339Nano, time.RFC3339} {
		if t, err := time.Parse(layout, s); err == nil {
			return t, nil
		}
	}
	return time.Time{}, fmt.Errorf("data %q não é uma data válida", s)
}

// Int64 parseia um valor de rota; em erro responde 400 e retorna false.
func Int64(w http.ResponseWriter, s string) (int64, bool) {
	id, err := strconv.ParseInt(s, 10, 64)
	if err != nil {
		slog.Error("id não é número", "valor", s)
		WriteStatusMessage(w, http.StatusBadRequest, "corpo inválido")
		return 0, false
	}
	return id, true
}

// FlexDate corresponde ao DateTime do C# aceito no bind: data simples
// ("2025-11-20") ou RFC3339. Serializa RFC3339 (padrão Go — B3).
type FlexDate struct{ time.Time }

func (d *FlexDate) UnmarshalJSON(data []byte) error {
	s := strings.TrimSpace(string(data))
	if s == "null" {
		return nil
	}
	if len(s) > 1 && s[0] == '"' {
		var str string
		if err := json.Unmarshal(data, &str); err != nil {
			return err
		}
		s = str
	}
	t, err := ParseDate(s)
	if err != nil {
		return err
	}
	d.Time = t
	return nil
}

func (d FlexDate) MarshalJSON() ([]byte, error) {
	b, err := json.Marshal(d.Format(time.RFC3339Nano))
	if err != nil {
		return nil, err
	}
	return b, nil
}

// StatusMapper decide o status code de um erro; handlers passam os seus sentinels.
type StatusMapper func(err error) (status int, ok bool)

func WriteStatusMessage(w http.ResponseWriter, status int, message string) {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	w.WriteHeader(status)
	// struct (não map) para ordem de campos determinística: {"status":…,"message":…}
	_ = json.NewEncoder(w).Encode(struct {
		Status  int    `json:"status"`
		Message string `json:"message"`
	}{status, message})
}

// Error traduz err em resposta JSON: tenta cada mapper em ordem;
// sem casamento, 500 com a PRÓPRIA mensagem do erro — o ErrorHandler do C#
// serializa ex.Message no 500 (message = ex.Message ?? "Erro Interno.").
func Error(w http.ResponseWriter, err error, mappers ...StatusMapper) {
	for _, mapper := range mappers {
		if status, ok := mapper(err); ok {
			WriteStatusMessage(w, status, err.Error())
			return
		}
	}
	slog.Error("erro inesperado", "err", err)
	WriteStatusMessage(w, http.StatusInternalServerError, err.Error())
}

// DecodeJSON decodifica o corpo; em falha responde 400 e retorna false.
// Divergência documentada: o C# devolve ValidationProblemDetails aqui.
func DecodeJSON(w http.ResponseWriter, r *http.Request, dst any) bool {
	if err := json.NewDecoder(r.Body).Decode(dst); err != nil {
		WriteStatusMessage(w, http.StatusBadRequest, "corpo inválido")
		return false
	}
	return true
}

func WriteJSON(w http.ResponseWriter, status int, body any) {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	w.WriteHeader(status)
	_ = json.NewEncoder(w).Encode(body)
}
