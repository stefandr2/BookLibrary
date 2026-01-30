# BookLibrary.Api

Sample ASP.NET Core Web API for a Book Library using EF Core and Azure SQL.

## Run locally
1. Install the .NET SDK (9.0+ recommended): https://dotnet.microsoft.com
2. From the project folder:
   - `dotnet restore`
   - (optional) Install EF CLI if you want to run migrations locally: `dotnet tool install --global dotnet-ef`
   - Create the initial EF migration: `dotnet ef migrations add Init`
   - Apply the migration to your local database: `dotnet ef database update`
   - Run the app: `dotnet run`



## Deploy to Azure (quick start)
These are the high-level steps. I can add scripts or GitHub Actions if you want.

1. Login to Azure CLI:
   - `az login`
2. Create a resource group:
   - `az group create -n BookLib-rg -l eastus`
3. Create an Azure SQL server and database (example):
   - `az sql server create -l eastus -g BookLib-rg -n booklib-server -u myadmin -p 'StrongPassword!'`
   - `az sql db create -g BookLib-rg -s booklib-server -n BookLibraryDb --service-objective Basic`
   - Configure firewall to allow your IP (or use private endpoints as needed):
     `az sql server firewall-rule create -g BookLib-rg -s booklib-server -n AllowMyIP --start-ip-address <your-ip> --end-ip-address <your-ip>`
4. Create an App Service and deploy:
   - `az appservice plan create -g BookLib-rg -n BookLibPlan --sku B1`
   - `az webapp create -g BookLib-rg -p BookLibPlan -n <your-app-name> --runtime "DOTNET|9.0"`
   - Set the connection string in the App Service (under Configuration > Connection strings) using the Azure SQL connection string (provide username/password).
   - Deploy using ZIP, `az webapp up`, or GitHub Actions.

## Migration & EF tools
- If `dotnet-ef` is not available globally, install it with `dotnet tool install --global dotnet-ef`.
- Alternatively, use the Package Manager Console or add migrations on CI before deploying.

If you want, I can add a GitHub Actions workflow for CI/CD and an `az` script to create resources automatically.