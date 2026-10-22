package movimentacoes

import "github.com/go-chi/chi/v5"

// Mount registra as rotas (réplica das classes de controller do C#).
func (h *Handler) Mount(r chi.Router) {
	r.Route("/api/movimentacoes", func(r chi.Router) {
		r.Get("/", h.listByPeriodo)
		r.Post("/", h.create)
		r.Post("/parcelas", h.subtrairParcelas)
		r.Get("/pendentes", h.pendentes)
		r.Post("/pendentes", h.processarPendentes)
		r.Delete("/{movimentacaoId}", h.undo)
		r.Get("/categorias", h.categorias)
		r.Route("/persistentes", func(r chi.Router) {
			r.Get("/{persistenteId}", h.getPersistente)
			r.Put("/", h.editPersistente)
			r.Delete("/{movimentacaoId}", h.deletarPersistente)
		})
	})
}
