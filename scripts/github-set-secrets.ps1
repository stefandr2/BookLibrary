Param(
    [Parameter(Mandatory = $true)]
    [string]$Repo,

    [Parameter(Mandatory = $true)]
    [string]$AzureCredentialsPath,

    [Parameter(Mandatory = $true)]
    [string]$ApiAppName,

    [Parameter(Mandatory = $true)]
    [string]$UiAppName
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $AzureCredentialsPath))
{
    throw "Azure credentials file not found: $AzureCredentialsPath"
}

Write-Host "Setting GitHub secrets on $Repo..."

$creds = Get-Content -Raw -Path $AzureCredentialsPath

& gh secret set AZURE_CREDENTIALS --repo $Repo --body $creds
& gh secret set API_WEBAPP_NAME --repo $Repo --body $ApiAppName
& gh secret set UI_WEBAPP_NAME --repo $Repo --body $UiAppName

Write-Host "Secrets configured."
