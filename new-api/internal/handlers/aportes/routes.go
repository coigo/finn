package aportes

import "github.com/go-chi/chi/v5"

// Mount registra as rotas (réplica das classes de controller do C#).
// O pânico de Money.Div (venda total em POST /) é convertido em 500 JSON
// pelo Recuperar global (Equivalent ao ErrorHandlerMiddleware único do C#),
// registrado no main.js — nada de middleware específico aqui.
func (h *Handler) Mount(r chi.Router) {
	r.Route("/api/aportes", func(r chi.Router) {
		r.Get("/", h.listar)
		r.Get("/categorias", h.categorias)
		r.Post("/", h.movimentar)
		r.Post("/{identificador}", h.movimentarPorIdentificador)
		r.Get("/{identificador}/historico", h.historico)
	})
}
