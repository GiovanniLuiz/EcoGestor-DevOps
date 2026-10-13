$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$target = Join-Path $root '.env'
if (Test-Path -LiteralPath $target) { throw '.env já existe. Preserve as credenciais do volume existente.' }
function New-Secret { -join (1..2 | ForEach-Object { [Guid]::NewGuid().ToString('N') }) }
$dbPassword = 'Eg9!' + (New-Secret)
$jwtKey = New-Secret
$adminPassword = 'Eg9!' + (New-Secret)
$content = @"
DB_PASSWORD=$dbPassword
JWT_KEY=$jwtKey
ADMIN_PASSWORD=$adminPassword
ADMIN_EMAIL=admin@ecogestor.example
APP_ENVIRONMENT=Development
API_PORT=8080
APP_IMAGE=ecogestor-api:local
APP_VERSION=local
"@
[IO.File]::WriteAllText($target, $content, [Text.UTF8Encoding]::new($false))
Write-Host '.env criado. Consulte ADMIN_EMAIL e ADMIN_PASSWORD nele para o login; não compartilhe esse arquivo.'
