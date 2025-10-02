Write-Host "=== Testing Jarvis Wake Word with Porcupine ===" -ForegroundColor Cyan
Write-Host ""

$settingsPath = "$env:APPDATA\HeyGPT\settings.json"
if (Test-Path $settingsPath) {
    $settings = Get-Content $settingsPath | ConvertFrom-Json
    Write-Host "Settings loaded:" -ForegroundColor Green
    Write-Host "  Wake Word: $($settings.WakeWord)"
    Write-Host "  Use Porcupine: $($settings.UsePorcupine)"
    Write-Host "  Sensitivity: $($settings.PorcupineSensitivity)"
    Write-Host "  AccessKey Present: $($settings.PicovoiceAccessKey -ne $null)"
    Write-Host ""
} else {
    Write-Host "Settings file not found" -ForegroundColor Red
    exit 1
}

$ppnPath = "C:\Users\SRGam\OneDrive\Documents\HeyGPT\ChatGptVoiceAssistant\bin\Release\net8.0-windows10.0.19041.0\resources\keyword_files\windows\jarvis_windows.ppn"
if (Test-Path $ppnPath) {
    $fileInfo = Get-Item $ppnPath
    Write-Host "jarvis_windows.ppn found:" -ForegroundColor Green
    Write-Host "  Path: $ppnPath"
    Write-Host "  Size: $($fileInfo.Length) bytes"
    Write-Host "  Modified: $($fileInfo.LastWriteTime)"
    Write-Host ""
} else {
    Write-Host "jarvis_windows.ppn NOT found" -ForegroundColor Red
    exit 1
}

$dllPath64 = "C:\Users\SRGam\OneDrive\Documents\HeyGPT\ChatGptVoiceAssistant\bin\Release\net8.0-windows10.0.19041.0\lib\windows\amd64\libpv_porcupine.dll"
$dllPathArm = "C:\Users\SRGam\OneDrive\Documents\HeyGPT\ChatGptVoiceAssistant\bin\Release\net8.0-windows10.0.19041.0\lib\windows\arm64\libpv_porcupine.dll"

if (Test-Path $dllPath64) {
    $dllInfo = Get-Item $dllPath64
    Write-Host "Porcupine DLL found (x64):" -ForegroundColor Green
    Write-Host "  Path: $dllPath64"
    Write-Host "  Size: $($dllInfo.Length) bytes"
    Write-Host ""
} elseif (Test-Path $dllPathArm) {
    $dllInfo = Get-Item $dllPathArm
    Write-Host "Porcupine DLL found (ARM64):" -ForegroundColor Green
    Write-Host "  Path: $dllPathArm"
    Write-Host "  Size: $($dllInfo.Length) bytes"
    Write-Host ""
} else {
    Write-Host "libpv_porcupine.dll NOT found in lib/windows" -ForegroundColor Red
    exit 1
}

Write-Host "=== All Jarvis Wake Word Files Present ===" -ForegroundColor Green
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Launch HeyGPT app"
Write-Host "2. Click Start Listening"
Write-Host "3. Say Jarvis clearly"
Write-Host "4. ChatGPT should launch automatically"
