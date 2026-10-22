// Package middleware reúne os transversais que o Program.cs registra:
// ErrorHandler (recover global em qualquer rota) e UseCors (política allowLocal).
package middleware

import (
	"fmt"
	"net/http"
	"strings"

	"github.com/coigo/finn/internal/platform/httpx"
)

// Recuperar replica o ErrorHandlerMiddleware C# no ramo default: qualquer
// pânico não tratado vira 500 {"status":500,"message":...} (o C# devolve
// ex.Message; o pânico de Go é o equivalente mais próximo).
func Recuperar() func(next http.Handler) http.Handler {
	return func(next http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			defer func() {
				if rec := recover(); rec != nil {
					httpx.WriteStatusMessage(w, http.StatusInternalServerError, fmt.Sprint(rec))
				}
			}()
			next.ServeHTTP(w, r)
		})
	}
}

// Cors replica a política allowLocal: origem WEB_URL, AllowCredentials,
// AllowAnyMethod, AllowAnyHeader; preflight (OPTIONS com Origin) -> 204.
func Cors(origem string) func(next http.Handler) http.Handler {
	return func(next http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			origemRequisitada := r.Header.Get("Origin")
			if strings.EqualFold(origemRequisitada, origem) {
				cab := w.Header()
				cab.Set("Access-Control-Allow-Origin", origem)
				cab.Set("Access-Control-Allow-Credentials", "true")
				if r.Method == http.MethodOptions {
					if pedidos := r.Header.Get("Access-Control-Request-Headers"); pedidos != "" {
						cab.Set("Access-Control-Allow-Headers", pedidos)
					}
					if metodo := r.Header.Get("Access-Control-Request-Method"); metodo != "" {
						cab.Set("Access-Control-Allow-Methods", metodo)
					}
					w.WriteHeader(http.StatusNoContent)
					return
				}
			}
			next.ServeHTTP(w, r)
		})
	}
}
