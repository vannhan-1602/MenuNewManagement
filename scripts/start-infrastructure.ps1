# Khởi động SQL Server + MongoDB + RabbitMQ cho demo mentor
Set-Location $PSScriptRoot\..

Write-Host "Kiem tra Docker Desktop dang chay..." -ForegroundColor Cyan
docker version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "LOI: Mo Docker Desktop truoc, roi chay lai script nay." -ForegroundColor Red
    exit 1
}

Write-Host "docker compose up -d ..." -ForegroundColor Cyan
docker compose up -d

Write-Host ""
Write-Host "Services:" -ForegroundColor Green
Write-Host "  SQL Server  -> localhost,1433 (sa / YourStrong@Passw0rd)"
Write-Host "  MongoDB     -> localhost:27017"
Write-Host "  RabbitMQ    -> localhost:5672 | UI http://localhost:15672 (guest/guest)"
Write-Host ""
Write-Host "Buoc tiep: chay database/01_CreateDatabase.sql tren localhost,1433"
Write-Host "Roi: cd src/Api && dotnet run --launch-profile Docker"
