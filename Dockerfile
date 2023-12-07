FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100 as build

WORKDIR /app

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

COPY ["FinaStrategy.Api/FinaStrategy.Api.csproj", "FinaStrategy.Api/"]
COPY ["FinaStrategy.Core/FinaStrategy.Core.csproj", "FinaStrategy.Core/"]
COPY ["FinaStrategy.Data/FinaStrategy.Data.csproj", "FinaStrategy.Data/"]
COPY ["FinaStrategy.Services/FinaStrategy.Services.csproj", "FinaStrategy.Services/"]

RUN dotnet restore "FinaStrategy.Api/FinaStrategy.Api.csproj"

COPY . .
WORKDIR /app/FinaStrategy.Api/
RUN dotnet publish FinaStrategy.Api.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/runtime:3.1.0-alpine3.10
WORKDIR /app

COPY  --from=build /app/FinaStrategy.Api/out ./

ENTRYPOINT ["dotnet", "FinaStrategy.Api.dll", "--server.urls", "http://0.0.0.0:5000"]