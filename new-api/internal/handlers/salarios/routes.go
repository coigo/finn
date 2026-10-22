package salarios

import "github.com/go-chi/chi/v5"

// Mount registra as rotas do domínio (replicação do Route("api/salario") C#).
func (h *Handler) Mount(r chi.Router) {
	r.Route("/api/salario", func(r chi.Router) {
		r.Get("/", h.getCurrent)
		r.Post("/", h.create)
		r.Post("/adicionar", h.payCurrent)
	})
}
