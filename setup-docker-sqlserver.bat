@echo off
echo ======================================
echo   Configurando SQL Server com Docker
echo ======================================
echo.

echo Verificando se Docker está instalado...
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERRO: Docker não encontrado!
    echo Por favor, instale o Docker Desktop primeiro:
    echo https://www.docker.com/products/docker-desktop
    pause
    exit /b 1
)

echo Docker encontrado! Verificando se já existe container...
docker ps -a --filter "name=sqlserver-devicemanagement" --format "{{.Names}}" | findstr sqlserver-devicemanagement >nul
if %errorlevel% equ 0 (
    echo Container já existe. Removendo container antigo...
    docker stop sqlserver-devicemanagement >nul 2>&1
    docker rm sqlserver-devicemanagement >nul 2>&1
)

echo.
echo Baixando e iniciando SQL Server 2022...
echo (Isso pode demorar alguns minutos na primeira vez)
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=DeviceManagement123!" -p 1433:1433 --name sqlserver-devicemanagement -d mcr.microsoft.com/mssql/server:2022-latest

if %errorlevel% equ 0 (
    echo.
    echo ✅ SQL Server iniciado com sucesso!
    echo.
    echo Aguardando SQL Server ficar pronto...
    timeout /t 15 /nobreak >nul

    echo.
    echo ==========================================
    echo   CONFIGURAÇÃO CONCLUÍDA!
    echo ==========================================
    echo.
    echo Connection String para usar no projeto:
    echo "Server=localhost,1433;Database=DeviceManagement;User Id=sa;Password=DeviceManagement123!;TrustServerCertificate=true;"
    echo.
    echo Comandos úteis:
    echo - Parar:    docker stop sqlserver-devicemanagement
    echo - Iniciar:  docker start sqlserver-devicemanagement
    echo - Status:   docker ps
    echo.
    echo Próximos passos:
    echo 1. Atualizar appsettings.json com a connection string
    echo 2. Executar: dotnet ef database update
    echo 3. Testar a API
    echo.
) else (
    echo.
    echo ❌ Erro ao iniciar SQL Server!
    echo Verifique se o Docker Desktop está rodando.
)

pause
