package saldos

import "github.com/go-chi/chi/v5"

// Mount registra as rotas do domínio (réplicas das rotas do C#).
func (h *Handler) Mount(r chi.Router) {
	r.Route("/api/saldos", func(r chi.Router) {
		r.Get("/", h.get)
		r.Post("/", h.set)
	})
}
