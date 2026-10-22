package movimentacoes

import (
	"context"
	"errors"
	"log/slog"
	"net/http"
	"time"

	"github.com/go-chi/chi/v5"

	dominio "github.com/coigo/finn/internal/domain/movimentacoes"
	"github.com/coigo/finn/internal/platform/httpx"
	"github.com/coigo/finn/internal/platform/money"
	servico "github.com/coigo/finn/internal/services/movimentacoes"
)

// ServiceAPI é o que o handler precisa do Service.
type ServiceAPI interface {
	Create(ctx context.Context, in servico.CriarInput) error
	SubtractParcelas(ctx context.Context, ids []int64) ([]int64, error)
	ListByPeriodo(ctx context.Context, inicio, fim time.Time) ([]dominio.Resumo, error)
	ListPendentes(ctx context.Context, hoje time.Time) ([]dominio.Pendente, string, error)
	ProcessPendentes(ctx context.Context, hoje time.Time) error
	Undo(ctx context.Context, id int64) (int64, error)
	ListCategorias(ctx context.Context) ([]dominio.Categoria, error)
	GetPersistente(ctx context.Context, id int64) (dominio.Persistente, error)
	EditPersistente(ctx context.Context, p dominio.Persistente) error
	SoftDeletePersistente(ctx context.Context, id int64, agora time.Time) (int64, error)
}

type Handler struct {
	svc ServiceAPI
	now func() time.Time // replay do DateTime.Now (injetável nos testes)
}

func NewHandler(svc ServiceAPI, now func() time.Time) *Handler {
	if now == nil {
		now = time.Now
	}
	return &Handler{svc: svc, now: now}
}

// status400 replica a regra do ErrorHandler: KeyNotFoundâ†’400 e, pela decisão
// B1, tudo que é erro de domínio (BusinessError equivalente) â†’ 400.
func status400(err error) (int, bool) {
	for _, s := range []error{
		dominio.ErrMovimentacaoNaoEncontrada,
		dominio.ErrMovimentacaoNaoEncontradaExcl,
		dominio.ErrMovimentacaoNaoEncontradaSemAcento,
	} {
		if is400(err, s) {
			return http.StatusBadRequest, true
		}
	}
	return 0, false
}

// is400 é errors.Is (shorthand das listas de sentinels acima).
func is400(err, target error) bool { return errors.Is(err, target) }

// criarField ecoa o request (replica o record CriarMovimentacao C# â€” campos
// na mesma ordem; tipo como número e datas RFC3339).
type criarCampoEcho struct {
	Valor              money.Money     `json:"valor"`
	Tipo               int64           `json:"tipo"`
	CategoriaID        int64           `json:"categoriaId"`
	Descricao          string          `json:"descricao"`
	Data               httpx.FlexDate  `json:"data"`
	QuantidadeParcelas *int64          `json:"quantidadeParcelas"`
	PrimeiroVencimento *httpx.FlexDate `json:"primeiroVencimento"`
	Persistente        *bool           `json:"persistente"`
}

func (e criarCampoEcho) paraInput() servico.CriarInput {
	return servico.CriarInput{
		Valor:              e.Valor,
		Tipo:               nomeDeTipo(e.Tipo),
		CategoriaID:        e.CategoriaID,
		Descricao:          e.Descricao,
		Data:               e.Data.Time,
		QuantidadeParcelas: e.QuantidadeParcelas,
		PrimeiroVencimento: flexPara(e.PrimeiroVencimento),
		Persistente:        e.Persistente,
	}
}

func flexPara(s *httpx.FlexDate) *time.Time {
	if s == nil {
		return nil
	}
	t := s.Time
	return &t
}

// nomeDeTipo converge númeroâ†’string (contrato JSON: 0|1|2).
func nomeDeTipo(tipo int64) string {
	switch tipo {
	case dominio.OrdinalEntrada:
		return dominio.TipoEntrada
	case dominio.OrdinalSaida:
		return dominio.TipoSaida
	case dominio.OrdinalInvestimentos:
		return dominio.TipoInvestimentos
	default:
		return ""
	}
}

// ordinalDoPeriodo replica a serialização do enum Periodo do C#
// (MES_ATUAL=0, PROXIMO_MES=1; qualquer outra coisa cai em 1 — o service
// só devolve as duas constantes).
func ordinalDoPeriodo(p string) int64 {
	if p == dominio.PeriodoMesAtual {
		return 0
	}
	return 1
}

// create = POST /api/movimentacoes (CriarMovimentacaoController).
func (h *Handler) create(w http.ResponseWriter, r *http.Request) {
	var echo criarCampoEcho
	if !httpx.DecodeJSON(w, r, &echo) {
		return
	}
	if err := h.svc.Create(r.Context(), echo.paraInput()); err != nil {
		httpx.Error(w, err, status400)
		return
	}
	// echo do request (C# retorna "data", sem esperar a gravação)
	httpx.WriteJSON(w, http.StatusOK, echo)
}

// subtrairParcelas = POST /api/movimentacoes/parcelas â€” echo de {parcelas:[â€¦]}.
func (h *Handler) subtrairParcelas(w http.ResponseWriter, r *http.Request) {
	var in struct {
		Parcelas []int64 `json:"parcelas"`
	}
	if !httpx.DecodeJSON(w, r, &in) {
		return
	}
	ids, err := h.svc.SubtractParcelas(r.Context(), in.Parcelas)
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, struct {
		Parcelas []int64 `json:"parcelas"`
	}{Parcelas: ids})
}

// listByPeriodo = GET /api/movimentacoes?inicio&fim.
func (h *Handler) listByPeriodo(w http.ResponseWriter, r *http.Request) {
	inicio, err := httpx.ParseDate(r.URL.Query().Get("inicio"))
	if err != nil {
		httpx.WriteStatusMessage(w, http.StatusBadRequest, "corpo inválido")
		return
	}
	fim, err := httpx.ParseDate(r.URL.Query().Get("fim"))
	if err != nil {
		httpx.WriteStatusMessage(w, http.StatusBadRequest, "corpo inválido")
		return
	}
	movs, err := h.svc.ListByPeriodo(r.Context(), inicio, fim)
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, movs)
}

// pendentes GET â€” BuscarMovimentacoesPendentesDoMesController.
func (h *Handler) pendentes(w http.ResponseWriter, r *http.Request) {
	lista, periodo, err := h.svc.ListPendentes(r.Context(), h.now())
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	// Paridade: Periodo do C# é enum — serializa como NÚMERO (MES_ATUAL=0,
	// PROXIMO_MES=1), não string.
	httpx.WriteJSON(w, http.StatusOK, struct {
		Pendentes []dominio.Pendente `json:"pendentes"`
		Periodo   int64              `json:"periodo"`
	}{lista, ordinalDoPeriodo(periodo)})
}

// processarPendentes = POST /api/movimentacoes/pendentes â†’ body true.
func (h *Handler) processarPendentes(w http.ResponseWriter, r *http.Request) {
	if err := h.svc.ProcessPendentes(r.Context(), h.now()); err != nil {
		httpx.Error(w, err, status400)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, true)
}

// undo = DELETE /api/movimentacoes/{movimentacaoId} â†’ body com o id.
func (h *Handler) undo(w http.ResponseWriter, r *http.Request) {
	id, ok := httpx.Int64(w, chi.URLParam(r, "movimentacaoId"))
	if !ok {
		return
	}
	movID, err := h.svc.Undo(r.Context(), id)
	if err != nil {
		// Paridade: o C# joga BusinessError("Movimentação não encontrada")
		// → ErrorHandler default → 500 com a mensagem.
		httpx.Error(w, err)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, movID)
}

// categoriaDTO replica BuscarMovimentacaoCategoriaDTO (JSON: id/name/tipo).
type categoriaDTO struct {
	ID   int64  `json:"id"`
	Name string `json:"name"`
	Tipo string `json:"tipo"`
}

// categorias = GET /api/movimentacoes/categorias.
func (h *Handler) categorias(w http.ResponseWriter, r *http.Request) {
	lista, err := h.svc.ListCategorias(r.Context())
	if err != nil {
		httpx.Error(w, err, status400)
		return
	}
	out := make([]categoriaDTO, 0, len(lista))
	for _, c := range lista {
		out = append(out, categoriaDTO{ID: c.ID, Name: c.Nome, Tipo: c.Tipo})
	}
	httpx.WriteJSON(w, http.StatusOK, out)
}

// getPersistente = GET /api/movimentacoes/persistentes/{id}.
func (h *Handler) getPersistente(w http.ResponseWriter, r *http.Request) {
	id, ok := httpx.Int64(w, chi.URLParam(r, "persistenteId"))
	if !ok {
		return
	}
	p, err := h.svc.GetPersistente(r.Context(), id)
	if err != nil {
		// Paridade: BusinessError("Movimentação não encontrada!") → 500.
		httpx.Error(w, err)
		return
	}
	httpx.WriteJSON(w, http.StatusOK, p)
}

// editPersistente = PUT /api/movimentacoes/persistentes â€” echo do request.
func (h *Handler) editPersistente(w http.ResponseWriter, r *http.Request) {
	var in struct {
		PersistenteID int64       `json:"persistenteId"`
		Valor         money.Money `json:"valor"`
		CategoriaID   int64       `json:"categoriaId"`
		Descricao     string      `json:"descricao"`
	}
	if !httpx.DecodeJSON(w, r, &in) {
		return
	}
	// Paridade: o C# chama o repo async void SEM await (EditarMovimentacaoPersistente)
	// → a resposta é sempre 200 + echo; uma falha (ex.: id inexistente — BusinessError)
	// vira exceção NÃO observada e mata o PROCESSO C#, nunca muda a resposta.
	// Go: repassa ao serviço; erro só vira log (não replicamos a queda do processo).
	if err := h.svc.EditPersistente(r.Context(), dominio.Persistente{
		ID:          in.PersistenteID,
		Valor:       in.Valor,
		Descricao:   in.Descricao,
		CategoriaID: in.CategoriaID,
	}); err != nil {
		slog.Error("editar persistente (fire-and-forget no C#)", "erro", err)
	}
	// echo (C# retorna o próprio DTO)
	httpx.WriteJSON(w, http.StatusOK, in)
}

// deletarPersistente = DELETE /api/movimentacoes/persistentes/{id} â†’ body id.
func (h *Handler) deletarPersistente(w http.ResponseWriter, r *http.Request) {
	id, ok := httpx.Int64(w, chi.URLParam(r, "movimentacaoId"))
	if !ok {
		return
	}
	movID, err := h.svc.SoftDeletePersistente(r.Context(), id, h.now())
	if err != nil {
		// Paridade: o C# chama o repo async void SEM await (DeletarMovimentacaoPersistente)
		// e retorna o PRÓPRIO id pedido mesmo sem achá-lo; a falha (BusinessError
		// async) mata o processo C# mas não muda a resposta. Go: repassa;
		// erro só vira log (não replicamos a queda do processo).
		slog.Error("deletar persistente (fire-and-forget no C#)", "id", id, "erro", err, "movID", movID)
	}
	httpx.WriteJSON(w, http.StatusOK, id)
}
