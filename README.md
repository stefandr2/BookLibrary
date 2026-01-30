# BookLibrary

Sample ASP.NET Core Web API for a Book Library. This repository contains the API project in `BookLibrary.Api`.

## Local development
- Ensure .NET 9 SDK is installed: https://dotnet.microsoft.com
- From the `BookLibrary.Api` folder:
  - `dotnet restore`
  - `dotnet ef migrations add Init` (optional)
  - `dotnet ef database update` (optional)
  - `dotnet run`

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

---

If you want, I can create the GitHub remote and set secrets once you log in; tell me when you're ready and I'll proceed. ✅
