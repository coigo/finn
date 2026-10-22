FROM node:22-alpine AS app-build

ARG API_BASE_URL
ENV VITE_API_BASE_URL="${API_BASE_URL}"

WORKDIR /app
COPY /app/package.json /app/yarn*.lock ./

RUN mkdir -p /app/node_modules && chown -R node:node /app

USER node
RUN npm i

COPY --chown=node:node /app .

RUN yarn build --debug

FROM golang:1.26.4-alpine AS api-build

WORKDIR /app
COPY /new-api/go.mod /new-api/go.sum ./
RUN go mod download

COPY /new-api ./

# CGO off: driver SQLite é o modernc.org/sqlite (pure Go) → binário estático
RUN CGO_ENABLED=0 go build -trimpath -ldflags="-s -w" -o ./api ./cmd/api

FROM alpine:3.22 AS runnable

WORKDIR /app

RUN addgroup -S --gid 1003 finn \
&& adduser -S -G finn --uid 1000 finn-user

RUN mkdir -p /app/data && chown -R finn-user:finn /app/data

# PORT, WEB_URL, B3_API_URL, B3_API_KEY, CRYPTO_API_URL e CRYPTO_API_KEY são
# OBRIGATÓRIAS (o Go falha no startup sem elas — config.Load, paridade do C#).
# Opcionais: DB_PATH (padrão data/finn.sqlite) e WEB_ROOT (padrão wwwroot).
ENV \
    PORT="5000" \
    WEB_URL="http://localhost:3000" \
    B3_API_URL="https://brapi.dev/api/quote" \
    B3_API_KEY="" \
    CRYPTO_API_URL="https://api.coingecko.com/api/v3/simple/price" \
    CRYPTO_API_KEY=""

COPY --from=api-build /app/api ./
COPY --from=app-build /app/dist/ ./wwwroot/

USER finn-user

EXPOSE 5000


# Sem ./api --migrate: no Go as migrations (goose) rodam no próprio startup
# (cmd/api/main.go run() aplica antes de servir).

CMD [ "./api" ]

# O modo ./api --migrate continua disponível para o initContainer do deploy.yaml.
