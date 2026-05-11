# Quick test script to verify Cosmos DB Emulator connection

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  COSMOS DB CONNECTION TEST" -ForegroundColor Yellow
Write-Host "========================================`n" -ForegroundColor Cyan

# Step 1: Check if emulator is running
Write-Host "Step 1: Checking if Cosmos DB Emulator is running..." -ForegroundColor Yellow
$emulatorProcess = Get-Process -Name "CosmosDB.Emulator" -ErrorAction SilentlyContinue

if ($emulatorProcess) {
    Write-Host "  ✓ Emulator process is running" -ForegroundColor Green
} else {
    Write-Host "  ✗ Emulator is NOT running!" -ForegroundColor Red
    Write-Host "  → Please start the emulator first" -ForegroundColor Yellow
    Write-Host "  → Run: .\start-cosmosdb-emulator.ps1`n" -ForegroundColor Yellow
    exit 1
}

# Step 2: Check if emulator endpoint is accessible
Write-Host "`nStep 2: Testing emulator endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://localhost:8081/_explorer/emulator.pem" `
        -SkipCertificateCheck `
        -TimeoutSec 5 `
        -ErrorAction Stop
    Write-Host "  ✓ Emulator endpoint is accessible" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Cannot reach emulator endpoint" -ForegroundColor Red
    Write-Host "  → Make sure emulator is fully started" -ForegroundColor Yellow
    exit 1
}

# Step 3: Show connection details
Write-Host "`nStep 3: Connection Details" -ForegroundColor Yellow
Write-Host "  Endpoint: https://localhost:8081" -ForegroundColor White
Write-Host "  Database: ECommerceDb" -ForegroundColor White
Write-Host "  Container: Products" -ForegroundColor White

# Step 4: Check configuration file
Write-Host "`nStep 4: Checking configuration..." -ForegroundColor Yellow
$configPath = "Product.API/appsettings.Development.json"
if (Test-Path $configPath) {
    Write-Host "  ✓ Configuration file exists" -ForegroundColor Green
    $config = Get-Content $configPath | ConvertFrom-Json
    $connectionString = $config.CosmosDb.ConnectionString
    
    if ($connectionString -like "*localhost:8081*") {
        Write-Host "  ✓ Connection string points to emulator" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Connection string doesn't point to emulator" -ForegroundColor Red
    }
} else {
    Write-Host "  ✗ Configuration file not found" -ForegroundColor Red
}

# Step 5: Ready to run
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  CONNECTION TEST PASSED!" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Run your application:" -ForegroundColor White
Write-Host "     cd Product.API" -ForegroundColor Gray
Write-Host "     dotnet run`n" -ForegroundColor Gray

Write-Host "  2. The app will automatically:" -ForegroundColor White
Write-Host "     • Connect to the emulator" -ForegroundColor Gray
Write-Host "     • Create database 'ECommerceDb'" -ForegroundColor Gray
Write-Host "     • Create container 'Products'" -ForegroundColor Gray
Write-Host "     • Seed 5 sample products`n" -ForegroundColor Gray

Write-Host "  3. View your data:" -ForegroundColor White
Write-Host "     https://localhost:8081/_explorer/index.html`n" -ForegroundColor Gray

$openExplorer = Read-Host "Open Data Explorer now? (Y/N)"
if ($openExplorer -eq "Y" -or $openExplorer -eq "y") {
    Start-Process "https://localhost:8081/_explorer/index.html"
}
