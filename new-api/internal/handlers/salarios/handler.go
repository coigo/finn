package salarios

import (
	"context"
	"errors"
	"net/http"

	dominio "github.com/coigo/finn/internal/domain/salarios"
	"github.com/coigo/finn/internal/platform/httpx"
	"github.com/coigo/finn/internal/platform/money"
)

// ServiceAPI é o que o handler precisa do service (Salarios.Service satisfaz).
type ServiceAPI interface {
	Create(ctx context.Context, valor money.Money) (dominio.Salario, error)
	GetCurrent(ctx context.Context) (dominio.Salario, error)
	PayCurrent(ctx context.Context) (dominio.Salario, error)
}

type Handler struct {
	svc ServiceAPI
}

func NewHandler(svc ServiceAPI) *Handler { return &Handler{svc: svc} }

// status400 replica: KeyNotFoundException→400; tudo mais→500 (paridade do
// ErrorHandler C#). "Use um valor positivo" é BusinessError → 500.
func status400(err error) (int, bool) {
	for _, s := range []error{dominio.ErrSalarioNaoCadastrado} {
		if is(err, s) {
			return http.StatusBadRequest, true
		}
	}
	return 0, false
}

// is é errors.Is (shorthand para leitura da switch acima).
func is(err, target error) bool { return errors.Is(err, target) }

// create = POST /api/salario {valor}.
func (h *Handler) create(w http.ResponseWriter, r *http.Request) {
	var in struct {
		Valor money.Money `json:"valor"`
	}
	if !httpx.DecodeJSON(w, r, &in) {
		return
	}
	salario, err := h.svc.Create(r.Context(), in.Valor)
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, salario)
}

// getCurrent = GET /api/salario.
func (h *Handler) getCurrent(w http.ResponseWriter, r *http.Request) {
	salario, err := h.svc.GetCurrent(r.Context())
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, salario)
}

// payCurrent = POST /api/salario/adicionar.
func (h *Handler) payCurrent(w http.ResponseWriter, r *http.Request) {
	salario, err := h.svc.PayCurrent(r.Context())
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, salario)
}
