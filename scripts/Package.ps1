param([string]$OutputPath = '')
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
if (-not $OutputPath) { $OutputPath = Join-Path (Split-Path $root -Parent) 'EcoGestor_DevOps_Entrega.zip' }
$output = [IO.Path]::GetFullPath($OutputPath)
if ($output.StartsWith([IO.Path]::GetFullPath($root) + [IO.Path]::DirectorySeparatorChar)) { throw 'Escolha uma saída fora da pasta do projeto.' }
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
$stream = [IO.File]::Open($output, [IO.FileMode]::CreateNew)
$archive = [IO.Compression.ZipArchive]::new($stream, [IO.Compression.ZipArchiveMode]::Create)
try {
    Get-ChildItem -LiteralPath $root -File -Recurse -Force | ForEach-Object {
        $rel = $_.FullName.Substring($root.Length + 1).Replace('\','/')
        if ($rel -match '(^|/)(bin|obj|\.git|\.vs|artifacts|TestResults)/') { return }
        if ($_.Name -like '.env*' -and $_.Name -ne '.env.example') { return }
        if ($_.Extension -in @('.zip','.tar')) { return }
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $_.FullName, "EcoGestorDevOps/$rel") | Out-Null
    }
} finally { $archive.Dispose(); $stream.Dispose() }
Write-Host "ZIP criado: $output"
