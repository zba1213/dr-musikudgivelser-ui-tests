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

Testene åbner Chrome i baggrunden. Den ene kontrollerer, at alle udgivelser vises. Den anden søger på titel og kunstner, kontrollerer beskeden ved nul match og nulstiller. Begge venter eksplicit på, at siden viser det forventede resultat.
