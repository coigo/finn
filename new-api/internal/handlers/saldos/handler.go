package saldos

import (
	"context"
	"errors"
	"net/http"

	dominio "github.com/coigo/finn/internal/domain/saldos"
	"github.com/coigo/finn/internal/platform/httpx"
	"github.com/coigo/finn/internal/platform/money"
)

// ServiceAPI é o que o handler precisa do service (saldos.Service satisfaz;
// definido no consumidor para os handler_test usarem fake).
type ServiceAPI interface {
	Get(ctx context.Context, nome string) (dominio.Saldo, error)
	Set(ctx context.Context, nome string, valor money.Money) (dominio.Saldo, error)
}

type Handler struct {
	svc ServiceAPI
}

func NewHandler(svc ServiceAPI) *Handler { return &Handler{svc: svc} }

// status400 mapeia os sentinels do domínio para 400 (B1).
func status400(err error) (int, bool) {
	if errors.Is(err, dominio.ErrSaldoNaoEncontrado) {
		return http.StatusBadRequest, true
	}
	return 0, false
}

// get = GET /api/saldos?saldoNome=… (BuscarSaldoController).
func (h *Handler) get(w http.ResponseWriter, r *http.Request) {
	nome := r.URL.Query().Get("saldoNome")
	saldo, err := h.svc.Get(r.Context(), nome)
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, saldo)
}

// set = POST /api/saldos (EditarSaldoController): {nome, valor}.
func (h *Handler) set(w http.ResponseWriter, r *http.Request) {
	var in struct {
		Nome  string      `json:"nome"`
		Valor money.Money `json:"valor"`
	}
	if !httpx.DecodeJSON(w, r, &in) {
		return
	}
	saldo, err := h.svc.Set(r.Context(), in.Nome, in.Valor)
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, saldo)
}
