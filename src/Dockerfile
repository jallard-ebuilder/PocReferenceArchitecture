FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReferenceArchitecture.Application/ReferenceArchitecture.Application.csproj", "ReferenceArchitecture.Application/"]
COPY ["ReferenceArchitecture.Core/ReferenceArchitecture.Core.csproj", "ReferenceArchitecture.Core/"]
COPY ["ReferenceArchitecture.Infrastructure/ReferenceArchitecture.Infrastructure.csproj", "ReferenceArchitecture.Infrastructure/"]
RUN dotnet restore "ReferenceArchitecture.Application/ReferenceArchitecture.Application.csproj"
COPY . .
WORKDIR /src/ReferenceArchitecture.Application
RUN dotnet build "ReferenceArchitecture.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReferenceArchitecture.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReferenceArchitecture.Application.dll"]
