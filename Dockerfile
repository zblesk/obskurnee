# Build backend
FROM mcr.microsoft.com/dotnet/sdk:5.0-windowsservercore-ltsc2019 AS dotnetbuild
WORKDIR /source

COPY *.sln .
COPY Obskurnee/*.csproj ./Obskurnee/
RUN dotnet restore  -r win-x64

COPY Obskurnee/. ./Obskurnee/
# This one can't be reffed via Nuget
COPY AspNetCore.Identity.LiteDB.dll ./Obskurnee/

WORKDIR /source/Obskurnee
RUN dotnet publish -c ReleaseNoNode -o /app --no-restore -r win-x64 --self-contained false

# Build FE
FROM node:14 AS nodebuild
WORKDIR /frontend
COPY Obskurnee/ClientApp/package*.json ./
RUN npm install
COPY Obskurnee/ClientApp/ .
RUN npm run build -- --prod

# final image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-windowsservercore-ltsc2019
WORKDIR /obskurnee
COPY --from=dotnetbuild /app ./
COPY --from=nodebuild /frontend/dist ./ClientApp
ENTRYPOINT ["Obskurnee"]