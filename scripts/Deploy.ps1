param(
    [Parameter(Mandatory=$true)][ValidateSet('staging','production')][string]$Target,
    [Parameter(Mandatory=$true)][string]$ImageArchive
)
$ErrorActionPreference = 'Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
foreach ($name in @('DB_PASSWORD','JWT_KEY','ADMIN_PASSWORD','APP_IMAGE','APP_VERSION','API_PORT','APP_ENVIRONMENT')) {
    if ([string]::IsNullOrWhiteSpace([Environment]::GetEnvironmentVariable($name))) { throw "Configuração ausente: $name" }
}
# A senha SQL entra em uma connection string. Use a geração hexadecimal descrita no guia.
if ($env:DB_PASSWORD -notmatch '^[A-Za-z0-9!_@#-]{16,128}$') { throw 'DB_PASSWORD deve usar 16 a 128 letras, números ou ! _ @ # -.' }
$project = "ecogestor-$Target"
$configRoot = Join-Path $env:LOCALAPPDATA 'EcoGestorDevOps'
New-Item -ItemType Directory -Force -Path $configRoot | Out-Null
$envFile = Join-Path $configRoot "$Target.env"
$lines = foreach ($name in @('DB_PASSWORD','JWT_KEY','ADMIN_PASSWORD','ADMIN_EMAIL','APP_IMAGE','APP_VERSION','API_PORT','APP_ENVIRONMENT')) {
    $value = [Environment]::GetEnvironmentVariable($name)
    if ($value -match "[\r\n']") { throw "Configuração inválida: $name" }
    "$name='$value'"
}
[IO.File]::WriteAllLines($envFile, $lines, [Text.UTF8Encoding]::new($false))
docker load --input $ImageArchive
if ($LASTEXITCODE -ne 0) { throw 'Falha ao carregar a imagem.' }
docker compose --env-file $envFile -p $project up -d --no-build --wait --wait-timeout 300
if ($LASTEXITCODE -ne 0) { throw 'Falha no deploy ou no healthcheck. A próxima etapa será bloqueada.' }
$baseUrl = "http://localhost:$($env:API_PORT)"
$ready = Invoke-RestMethod "$baseUrl/health/ready"
if ($ready.environment -ne $env:APP_ENVIRONMENT -or $ready.version -ne $env:APP_VERSION) { throw 'Ambiente ou versão incorretos.' }
$units = Invoke-RestMethod "$baseUrl/api/unidades"
if ($units.totalItems -lt 1) { throw 'Dados iniciais não encontrados.' }
$body = @{ email=$env:ADMIN_EMAIL; senha=$env:ADMIN_PASSWORD } | ConvertTo-Json
$auth = Invoke-RestMethod "$baseUrl/api/auth/login" -Method Post -ContentType 'application/json' -Body $body
if (-not $auth.token) { throw 'Login não retornou token.' }
$report = Invoke-RestMethod "$baseUrl/api/relatorios/impacto" -Headers @{ Authorization="Bearer $($auth.token)" }
New-Item -ItemType Directory -Force artifacts | Out-Null
@{ ambiente=$Target; url=$baseUrl; commit=$env:APP_VERSION; verificadoEmUtc=[DateTime]::UtcNow.ToString('o'); health=$ready; unidades=$units.totalItems; relatorio=$report } |
    ConvertTo-Json -Depth 6 | Set-Content -Encoding UTF8 "artifacts/deploy-$Target.json"
Write-Host "Deploy verificado: $Target em $baseUrl"
