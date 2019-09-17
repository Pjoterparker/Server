﻿param(
[Parameter(Mandatory=$false)][string]$buildPath,
[string]$configuration,
[string]$migration
)

if(!($buildPath)){
    $buildPath = Split-Path -Path $PSScriptRoot -Parent
}
else
{
    $buildPath = Join-Path $buildPath 'PjoterParker.Database'
}

$Env:ASPNETCORE_ENVIRONMENT = $configuration
Set-Location $buildPath
dotnet ef migrations add $migration --startup-project ../PjoterParker.Api --verbose
