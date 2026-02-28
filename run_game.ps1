$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $repoRoot

$candidates = @(
    "Builds\Windows\LoxQuest3D.exe",
    "Builds\Windows\LoxQuest3D\LoxQuest3D.exe",
    "Build\LoxQuest3D.exe",
    "LoxQuest3D.exe"
)

$exe = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $exe) {
    Write-Host "Не найден билд игры (.exe)." -ForegroundColor Yellow
    Write-Host "Собери билд в Unity: File -> Build Settings -> Build, и положи его в Builds\Windows\LoxQuest3D.exe" -ForegroundColor Yellow
    exit 1
}

Write-Host "Запуск: $exe"
Start-Process -FilePath $exe -WorkingDirectory (Split-Path -Parent $exe)

