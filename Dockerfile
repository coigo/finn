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

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.22 AS api-build

WORKDIR /app
COPY /api/ ./

RUN dotnet restore
RUN dotnet publish -c Release -o ./publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.22 AS runnable

WORKDIR /app

RUN addgroup -S --gid 1003 finn \
&& adduser -S -G finn --uid 1000 finn-user

RUN apk add curl

COPY --from=api-build /app/publish ./
COPY --from=app-build /app/dist/ ./wwwroot/
RUN mkdir -p /app/data && chown -R finn-user:finn /app/data

USER finn-user

EXPOSE 5000


CMD [ "dotnet", "api.dll" ]
