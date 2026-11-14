# Saare Revit versions ke liye Release build banata hai.
# Chalane ke liye:  powershell -ExecutionPolicy Bypass -File build\build-all.ps1
$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot "..\src\MyRevitTools\MyRevitTools.csproj"

foreach ($version in 2023, 2024, 2025, 2026) {
    Write-Host "=== Building for Revit $version ===" -ForegroundColor Cyan
    dotnet build $project -c Release -p:RevitVersion=$version
    if ($LASTEXITCODE -ne 0) { throw "Build failed for Revit $version" }
}

Write-Host "Done. Output: src\MyRevitTools\bin\Release\R<version>\" -ForegroundColor Green
