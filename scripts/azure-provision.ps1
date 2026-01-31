Param(
    [Parameter(Mandatory = $true)]
    [string]$SubscriptionId,

    [Parameter(Mandatory = $true)]
    [string]$Location,

    [Parameter(Mandatory = $true)]
    [string]$ResourceGroup,

    [Parameter(Mandatory = $true)]
    [string]$ApiAppName,

    [Parameter(Mandatory = $true)]
    [string]$UiAppName,

    [Parameter(Mandatory = $false)]
    [string]$SqlServerName,

    [Parameter(Mandatory = $false)]
    [string]$SqlAdminUser,

    [Parameter(Mandatory = $false)]
    [string]$SqlAdminPassword,

    [Parameter(Mandatory = $false)]
    [string]$SqlDatabaseName,

    [Parameter(Mandatory = $false)]
    [switch]$SkipSqlProvisioning
)

$ErrorActionPreference = "Stop"

$dotnetRuntime = "dotnet:9"

Write-Host "Setting subscription..."
az account set --subscription $SubscriptionId

Write-Host "Creating resource group..."
az group create --name $ResourceGroup --location $Location

Write-Host "Creating App Service plan..."
az appservice plan create --name "${ResourceGroup}-plan" --resource-group $ResourceGroup --location $Location --sku B1

Write-Host "Creating API Web App..."
az webapp create --name $ApiAppName --resource-group $ResourceGroup --plan "${ResourceGroup}-plan" --runtime $dotnetRuntime

Write-Host "Creating UI Web App..."
az webapp create --name $UiAppName --resource-group $ResourceGroup --plan "${ResourceGroup}-plan" --runtime $dotnetRuntime

if (-not $SkipSqlProvisioning)
{
    if ([string]::IsNullOrWhiteSpace($SqlServerName) -or [string]::IsNullOrWhiteSpace($SqlAdminUser) -or [string]::IsNullOrWhiteSpace($SqlAdminPassword) -or [string]::IsNullOrWhiteSpace($SqlDatabaseName))
    {
        throw "SqlServerName, SqlAdminUser, SqlAdminPassword, and SqlDatabaseName are required unless -SkipSqlProvisioning is specified."
    }

    Write-Host "Creating Azure SQL server..."
    az sql server create --name $SqlServerName --resource-group $ResourceGroup --location $Location --admin-user $SqlAdminUser --admin-password $SqlAdminPassword

    Write-Host "Creating Azure SQL database..."
    az sql db create --resource-group $ResourceGroup --server $SqlServerName --name $SqlDatabaseName --service-objective Basic

    Write-Host "Allow Azure services to access SQL server..."
    az sql server firewall-rule create --resource-group $ResourceGroup --server $SqlServerName --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
}

Write-Host "Provisioning complete. Next, set app settings and GitHub secrets."
