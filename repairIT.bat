@echo off
chcp 65001 >nul
cd /d "%~dp0"

dotnet build RepairIT.slnx
if errorlevel 1 (
    echo La compilation a echoue.
    pause
    exit /b 1
)

start "RepairIT.CatalogService" cmd /k dotnet run --no-build --project RepairIT.CatalogService --launch-profile http
start "RepairIT.UserService" cmd /k dotnet run --no-build --project RepairIT.UserService --launch-profile http
start "RepairIT.RepairService" cmd /k dotnet run --no-build --project RepairIT.RepairService --launch-profile http
start "RepairIT.SalesService" cmd /k dotnet run --no-build --project RepairIT.SalesService --launch-profile http
start "RepairIT.AuthentificationService" cmd /k dotnet run --no-build --project RepairIT.AuthentificationService --launch-profile http
start "RepairIT.Client" cmd /k dotnet run --no-build --project RepairIT.Client --launch-profile http

echo.
echo CatalogService : http://localhost:5234/swagger
echo UserService    : http://localhost:5005/swagger
echo Client Blazor  : http://localhost:5228
