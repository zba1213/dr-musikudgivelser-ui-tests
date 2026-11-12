# Musikudgivelser UI-test

Start API og frontend før testen. Du skal have .NET 10, Chrome og internetadgang til NuGet og til Selenium Manager ved første hentning af ChromeDriver.

```powershell
dotnet test MusicRecords.UiTests.sln
```

Standardadressen er `http://localhost:5500`. Sæt miljøvariablen `FRONTEND_URL` i samme terminal for at teste den publicerede frontend.

```powershell
$env:FRONTEND_URL = "https://dit-frontend-hostnavn"
dotnet test MusicRecords.UiTests.sln
```

Testen åbner Chrome i baggrunden, venter på tre synlige rækker og kontrollerer de fire viste felter for hver udgivelse.
