FROM node:22-alpine AS app-build

WORKDIR /app
COPY /app/package.json /app/yarn*.lock ./

RUN mkdir -p /app/node_modules && chown -R node:node /app

USER node
RUN yarn

COPY --chown=node:node /app .

RUN yarn build --debug

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.22 AS api-build

WORKDIR /app
COPY /api/ ./

COPY --from=app-build /app/dist/ ./src/wwwroot/

RUN dotnet restore
RUN dotnet publish -c Release -o ./publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.22 AS runnable

WORKDIR /app

RUN addgroup -S finn \
    && adduser -S -G finn finn-user

RUN apk add curl
    
COPY --from=api-build /app/publish ./
USER finn-user

EXPOSE 5000

CMD [ "dotnet", "api.dll" ]