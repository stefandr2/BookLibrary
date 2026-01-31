# BookLibrary

ASP.NET Core Book Library with an API and a Blazor UI:
- API: `BookLibrary.Api`
- UI: `BookLibrary.Ui` (Blazor Web App with server interactivity)

## Local development
- Ensure .NET 9 SDK is installed: https://dotnet.microsoft.com
- From the `BookLibrary.Api` folder:
   - `dotnet restore`
   - `dotnet ef migrations add Init` (optional)
   - `dotnet ef database update` (optional)
   - `dotnet run`
- From the `BookLibrary.Ui` folder:
   - `dotnet restore`
   - `dotnet run`
   - Update `BookLibrary.Ui/appsettings.Development.json` `ApiBaseUrl` if the API runs on a different URL.

---

## GitHub & Azure setup (manual steps)
Since you are not logged into `gh` or `az`, complete these steps locally when ready:

1. Login to GitHub CLI:
   - `gh auth login`
2. Create the remote repository (from repository root):
   - `gh repo create BookLibrary --public --source=. --remote=origin --push`

3. Login to Azure CLI:
   - `az login`

4. Create an Azure service principal for GitHub Actions and save credentials:
   - `az ad sp create-for-rbac --name "BookLibrary-GitHub" --role contributor --scopes /subscriptions/<subscription-id> --sdk-auth > azure_credentials.json`
   - Add to GitHub secrets (replace `<repo>` and run from repo root):
     - `gh secret set AZURE_CREDENTIALS --repo <owner>/<repo> --body "$(cat azure_credentials.json)"`
     - `gh secret set WEBAPP_NAME --repo <owner>/<repo> --body "<your-app-service-name>"`

5. After setting secrets, push to `main` and Actions will run for CI/CD.

### Required GitHub secrets for Azure deploy
- `AZURE_CREDENTIALS` (service principal JSON)
- `API_WEBAPP_NAME` (Azure App Service name for the API)
- `UI_WEBAPP_NAME` (Azure App Service name for the UI)

### App settings to configure in Azure
- API App Service connection string: `DefaultConnection` (Azure SQL)
- UI App Service setting: `ApiBaseUrl` (points to the API URL)

## Azure provisioning scripts
PowerShell scripts are available in [scripts/azure-provision.ps1](scripts/azure-provision.ps1) and [scripts/azure-configure-appsettings.ps1](scripts/azure-configure-appsettings.ps1).

Example usage:
- Create resources:
   - `./scripts/azure-provision.ps1 -SubscriptionId <sub> -Location eastus -ResourceGroup BookLib-rg -ApiAppName <api-app> -UiAppName <ui-app> -SqlServerName <sql-server> -SqlAdminUser <user> -SqlAdminPassword <password> -SqlDatabaseName BookLibraryDb`
- Configure app settings:
   - `./scripts/azure-configure-appsettings.ps1 -SubscriptionId <sub> -ResourceGroup BookLib-rg -ApiAppName <api-app> -UiAppName <ui-app> -SqlServerName <sql-server> -SqlDatabaseName BookLibraryDb -SqlAdminUser <user> -SqlAdminPassword <password>`

### Using SQL Server on a VM
If you host SQL Server 2019 on a VM (e.g., `booklibsql2026`), skip Azure SQL provisioning and point the API at the VM host:
- `./scripts/azure-provision.ps1 -SubscriptionId <sub> -Location eastus -ResourceGroup BookLib-rg -ApiAppName <api-app> -UiAppName <ui-app> -SkipSqlProvisioning`
- `./scripts/azure-configure-appsettings.ps1 -SubscriptionId <sub> -ResourceGroup BookLib-rg -ApiAppName <api-app> -UiAppName <ui-app> -SqlServerHost booklibsql2026 -SqlPort 1433 -SqlDatabaseName BookLibraryDb -SqlAdminUser <user> -SqlAdminPassword <password>`

Ensure the VM allows inbound TCP 1433 from the App Service outbound IPs and SQL Server TCP/IP is enabled.

## GitHub secrets helper
Use [scripts/github-set-secrets.ps1](scripts/github-set-secrets.ps1) after creating a service principal JSON.

Example usage:
- `./scripts/github-set-secrets.ps1 -Repo <owner>/<repo> -AzureCredentialsPath ./azure_credentials.json -ApiAppName <api-app> -UiAppName <ui-app>`

---

If you want, I can create the GitHub remote and set secrets once you log in; tell me when you're ready and I'll proceed. ✅
