Param(
    [Parameter(Mandatory = $true)]
    [string]$SubscriptionId,

    [Parameter(Mandatory = $true)]
    [string]$ResourceGroup,

    [Parameter(Mandatory = $true)]
    [string]$ApiAppName,

    [Parameter(Mandatory = $true)]
    [string]$UiAppName,

    [Parameter(Mandatory = $false)]
    [string]$SqlServerName,

    [Parameter(Mandatory = $false)]
    [string]$SqlServerHost,

    [Parameter(Mandatory = $false)]
    [int]$SqlPort = 1433,

    [Parameter(Mandatory = $true)]
    [string]$SqlDatabaseName,

    [Parameter(Mandatory = $true)]
    [string]$SqlAdminUser,

    [Parameter(Mandatory = $true)]
    [string]$SqlAdminPassword
)

$ErrorActionPreference = "Stop"

Write-Host "Setting subscription..."
az account set --subscription $SubscriptionId

if ([string]::IsNullOrWhiteSpace($SqlServerName) -and [string]::IsNullOrWhiteSpace($SqlServerHost))
{
    throw "Provide SqlServerName for Azure SQL or SqlServerHost for SQL Server VM."
}

if (-not [string]::IsNullOrWhiteSpace($SqlServerHost))
{
    $connectionString = "Server=tcp:$SqlServerHost,$SqlPort;Initial Catalog=$SqlDatabaseName;Persist Security Info=False;User ID=$SqlAdminUser;Password=$SqlAdminPassword;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;"
    $connectionStringType = "SQLServer"
}
else
{
    $connectionString = "Server=tcp:$SqlServerName.database.windows.net,1433;Initial Catalog=$SqlDatabaseName;Persist Security Info=False;User ID=$SqlAdminUser;Password=$SqlAdminPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    $connectionStringType = "SQLAzure"
}

Write-Host "Setting API connection string..."
az webapp config connection-string set --resource-group $ResourceGroup --name $ApiAppName --settings DefaultConnection="$connectionString" --connection-string-type $connectionStringType

$apiBaseUrl = "https://$ApiAppName.azurewebsites.net"

Write-Host "Setting UI ApiBaseUrl..."
az webapp config appsettings set --resource-group $ResourceGroup --name $UiAppName --settings ApiBaseUrl=$apiBaseUrl

Write-Host "App settings configured."
