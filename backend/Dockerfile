# ── Stage 1: Build ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar .sln y todos los .csproj primero para cachear el restore
COPY Multibanca.Backend.sln global.json ./
COPY Common.Application.Implementations/Common.Application.Implementations.csproj Common.Application.Implementations/
COPY Common.Application.Interfaces/Common.Application.Interfaces.csproj Common.Application.Interfaces/
COPY Common.Domain.Models/Common.Domain.Models.csproj Common.Domain.Models/
COPY Data.Extensions.Repository/Data.Extensions.Repository.csproj Data.Extensions.Repository/
COPY Data.Repository.Implementations/Data.Repository.Implementations.csproj Data.Repository.Implementations/
COPY Data.Repository.Interfaces/Data.Repository.Interfaces.csproj Data.Repository.Interfaces/
COPY Framework.WorkFlow.Application.Implementations/Framework.WorkFlow.Application.Implementations.csproj Framework.WorkFlow.Application.Implementations/
COPY Framework.WorkFlow.Application.Interfaces/Framework.WorkFlow.Application.Interfaces.csproj Framework.WorkFlow.Application.Interfaces/
COPY Framework.WorkFlow.Common.DTO/Framework.WorkFlow.Common.DTO.csproj Framework.WorkFlow.Common.DTO/
COPY Framework.WorkFlow.Repository.Implementation/Framework.WorkFlow.Repository.Implementation.csproj Framework.WorkFlow.Repository.Implementation/
COPY Framework.WorkFlow.Repository.Interface/Framework.WorkFlow.Repository.Interface.csproj Framework.WorkFlow.Repository.Interface/
COPY Multibanca.Application.Implementations/Multibanca.Application.Implementations.csproj Multibanca.Application.Implementations/
COPY Multibanca.Application.Interfaces/Multibanca.Application.Interfaces.csproj Multibanca.Application.Interfaces/
COPY Multibanca.Automapper/Multibanca.Automapper.csproj Multibanca.Automapper/
COPY Multibanca.Backend.Api/Multibanca.Backend.Api.csproj Multibanca.Backend.Api/
COPY Multibanca.Common/Multibanca.Common.csproj Multibanca.Common/
COPY Multibanca.Domain.Models/Multibanca.Domain.Models.csproj Multibanca.Domain.Models/
COPY Multibanca.DTO/Multibanca.DTO.csproj Multibanca.DTO/
COPY Multibanca.Register.IoC/Multibanca.Register.IoC.csproj Multibanca.Register.IoC/

RUN dotnet restore Multibanca.Backend.sln

COPY . .

RUN dotnet publish Multibanca.Backend.Api/Multibanca.Backend.Api.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ───────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0
# Drivers ODBC necesarios para la conexión multibanca_odbc de BoldReports.
# En Linux el driver se llama "PostgreSQL Unicode" (sin "(x64)").
# libfontconfig1 es requerido en runtime por libSkiaSharp.so (Syncfusion.SkiaSharpHelper.Net.Core);
# sin ella falla la carga nativa con TypeInitializationException en SKImageInfo.
# fonts-liberation/ttf-mscorefonts-installer: sin fuentes reales instaladas, DocIORenderer
# hace fallback exhaustivo de glifos y la conversión DOCX->PDF puede tardar minutos.
RUN apt-get update && apt-get install -y --no-install-recommends \
    unixodbc \
    odbc-postgresql \
    libfontconfig1 \
    fontconfig \
    fonts-liberation \
    fonts-crosextra-carlito \
    fonts-crosextra-caladea \
    fonts-dejavu-core \
    && fc-cache -f \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
#ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "Multibanca.Backend.Api.dll"]
