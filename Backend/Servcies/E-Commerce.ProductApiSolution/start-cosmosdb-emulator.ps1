# PowerShell script to start and verify Cosmos DB Emulator

Write-Host "Cosmos DB Emulator Setup Script" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Check if emulator is installed
$emulatorPath = "C:\Program Files\Azure Cosmos DB Emulator\CosmosDB.Emulator.exe"

if (-not (Test-Path $emulatorPath)) {
    Write-Host "ERROR: Cosmos DB Emulator not found at: $emulatorPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install the Cosmos DB Emulator from:" -ForegroundColor Yellow
    Write-Host "https://aka.ms/cosmosdb-emulator" -ForegroundColor Yellow
    Write-Host ""
    exit 1
}

Write-Host "✓ Cosmos DB Emulator found" -ForegroundColor Green

# Check if emulator is already running
$emulatorProcess = Get-Process -Name "CosmosDB.Emulator" -ErrorAction SilentlyContinue

if ($emulatorProcess) {
    Write-Host "✓ Cosmos DB Emulator is already running" -ForegroundColor Green
} else {
    Write-Host "Starting Cosmos DB Emulator..." -ForegroundColor Yellow
    Start-Process $emulatorPath -ArgumentList "/NoUI"
    Write-Host "Waiting for emulator to start (this may take 30-60 seconds)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
}

# Wait for emulator to be ready
Write-Host ""
Write-Host "Checking emulator status..." -ForegroundColor Yellow

$maxAttempts = 30
$attempt = 0
$isReady = $false

while ($attempt -lt $maxAttempts -and -not $isReady) {
    try {
        $attempt++
        Write-Host "Attempt $attempt/$maxAttempts..." -ForegroundColor Gray
        
        # Try to connect to the emulator endpoint
        $response = Invoke-WebRequest -Uri "https://localhost:8081/_explorer/emulator.pem" `
            -SkipCertificateCheck `
            -TimeoutSec 2 `
            -ErrorAction Stop
        
        if ($response.StatusCode -eq 200) {
            $isReady = $true
        }
    } catch {
        Start-Sleep -Seconds 2
    }
}

Write-Host ""
if ($isReady) {
    Write-Host "✓ Cosmos DB Emulator is ready!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Emulator Details:" -ForegroundColor Cyan
    Write-Host "  Endpoint: https://localhost:8081" -ForegroundColor White
    Write-Host "  Data Explorer: https://localhost:8081/_explorer/index.html" -ForegroundColor White
    Write-Host ""
    Write-Host "You can now run your application:" -ForegroundColor Yellow
    Write-Host "  cd Product.API" -ForegroundColor White
    Write-Host "  dotnet run" -ForegroundColor White
    Write-Host ""
    
    # Ask if user wants to open Data Explorer
    $openExplorer = Read-Host "Open Data Explorer in browser? (Y/N)"
    if ($openExplorer -eq "Y" -or $openExplorer -eq "y") {
        Start-Process "https://localhost:8081/_explorer/index.html"
    }
} else {
    Write-Host "✗ Failed to connect to Cosmos DB Emulator" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting steps:" -ForegroundColor Yellow
    Write-Host "1. Check if the emulator process is running in Task Manager" -ForegroundColor White
    Write-Host "2. Try restarting the emulator manually" -ForegroundColor White
    Write-Host "3. Check Windows Event Viewer for errors" -ForegroundColor White
    Write-Host "4. Ensure port 8081 is not being used by another application" -ForegroundColor White
    exit 1
}
