FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
WORKDIR /source

COPY . .
RUN  ls -alr 
RUN rm -rf obskurnee.client
RUN  ls -alr 
WORKDIR /source/Obskurnee.Server
RUN dotnet publish -a $TARGETARCH -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
RUN mkdir /app/wwwroot
RUN echo "Hello <b>world</b>. Built: <br>" > /app/wwwroot/index.html
RUN date >> /app/wwwroot/index.html

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8
ENTRYPOINT ["./Obskurnee.Server"]