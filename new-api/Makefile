# github.com/coigo/finn — atalhos de desenvolvimento (PowerShell: use `make` via Git Bash ou choco).
.PHONY: sqlc migrate test lint build run

sqlc:
	sqlc generate && sqlc vet

migrate:
	go run ./cmd/api --migrate

test:
	go test ./...

lint:
	gofmt -l . && go vet ./...

build:
	go build ./cmd/api

run:
	go run ./cmd/api
