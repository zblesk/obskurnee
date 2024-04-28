FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get install nodejs -y

WORKDIR /source
COPY . .

RUN dotnet restore -r linux-musl-x64 ./Obskurnee.Server/Obskurnee.Server.csproj
RUN dotnet restore -r linux-musl-x64 ./obskurnee.client/obskurnee.client.esproj

RUN  ls -alr 

WORKDIR /source/obskurnee.client
RUN npm ci

WORKDIR /source/Obskurnee.Server

RUN dotnet publish --no-restore -a musl-x64 -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
RUN echo "Built: <br><b>" > /app/wwwroot/build.html
RUN date >> /app/wwwroot/build.html
RUN echo "</b>" >> /app/wwwroot/build.html

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8
ENTRYPOINT ["./Obskurnee.Server"]