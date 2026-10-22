// Package config carrega a configuração do processo a partir do ambiente —
// réplica do Program.cs/LoadEnv do C# (PORT, WEB_URL, chaves de cotação).
package config

import (
	"errors"
	"fmt"
	"os"
)

type Config struct {
	Port    string // escuta HTTP (0.0.0.0:PORT, como o Kestrel do C#)
	WebURL  string // origem CORS do SPA
	DBPath  string // arquivo SQLite (padrão data/finn.sqlite)
	WebRoot string // diretório do SPA servido pela API (padrão wwwroot)
	B3      Upstream
	Crypto  Upstream
}

type Upstream struct {
	URL string
	Key string
}

// Load lê as variáveis de ambiente; faltou variável obrigatória -> erro
// (o C# também falha no startup — paridade de comportamento).
func Load() (Config, error) {
	var cfg Config
	var err error
	if cfg.Port, err = required("PORT"); err != nil {
		return cfg, err
	}
	if cfg.WebURL, err = required("WEB_URL"); err != nil {
		return cfg, err
	}
	if cfg.B3.URL, err = required("B3_API_URL"); err != nil {
		return cfg, err
	}
	if cfg.B3.Key, err = required("B3_API_KEY"); err != nil {
		return cfg, err
	}
	if cfg.Crypto.URL, err = required("CRYPTO_API_URL"); err != nil {
		return cfg, err
	}
	if cfg.Crypto.Key, err = required("CRYPTO_API_KEY"); err != nil {
		return cfg, err
	}
	cfg.DBPath = os.Getenv("DB_PATH")
	if cfg.DBPath == "" {
		cfg.DBPath = "data/finn.sqlite"
	}
	cfg.WebRoot = os.Getenv("WEB_ROOT")
	if cfg.WebRoot == "" {
		cfg.WebRoot = "wwwroot"
	}
	return cfg, nil
}

func required(nome string) (string, error) {
	v := os.Getenv(nome)
	if v == "" {
		return "", fmt.Errorf("variável de ambiente obrigatória ausente: %s", nome)
	}
	return v, nil
}

var ErrConfig = errors.New("configuração inválida")
