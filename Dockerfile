FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

    RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash -
    RUN apt-get install nodejs git-all -y

    WORKDIR /source
    COPY . .
    RUN dotnet restore -r linux-musl-x64 ./Obskurnee.Server/Obskurnee.Server.csproj
    RUN dotnet restore -r linux-musl-x64 ./obskurnee.client/obskurnee.client.esproj

    WORKDIR /source/obskurnee.client
    RUN npm ci

    WORKDIR /source/Obskurnee.Server
    RUN dotnet publish --no-restore -a musl-x64 -o /app

    RUN git log -n 1 > /app/gitstatus

# final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

    EXPOSE 8080
    WORKDIR /app
    COPY --from=build /app .

    RUN echo "Built: <br><b>" > /app/wwwroot/build.html
    RUN date >> /app/wwwroot/build.html
    RUN echo "</b><br /><br />Git commit: <br />" >> /app/wwwroot/build.html
    RUN cat /app/gitstatus >> /app/wwwroot/build.html
    RUN rm /app/gitstatus

    ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
    ENV LC_ALL=en_US.UTF-8
    ENV LANG=en_US.UTF-8
    ENTRYPOINT ["./Obskurnee.Server"]