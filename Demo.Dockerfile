# Build BE
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS dotnetbuild
WORKDIR /source

COPY *.sln .
COPY Obskurnee/*.csproj ./Obskurnee/
RUN dotnet restore -r linux-musl-x64

COPY Obskurnee/. ./Obskurnee/

WORKDIR /source/Obskurnee
RUN dotnet publish -c DemoReleaseNoNode -o /app --no-restore -r linux-musl-x64 --self-contained false

# Build FE
FROM node:14 AS nodebuild
WORKDIR /frontend
COPY Obskurnee/ClientApp/package*.json ./
RUN npm install
COPY Obskurnee/ClientApp/ .
RUN npm run build -- --prod

# final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64
WORKDIR /obskurnee
COPY --from=dotnetbuild /app ./
COPY --from=nodebuild /frontend/dist ./ClientApp/dist
COPY Demo/obskurnee.db ./data/
COPY Demo/appsettings.json ./

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8
ENTRYPOINT ["./Obskurnee"]