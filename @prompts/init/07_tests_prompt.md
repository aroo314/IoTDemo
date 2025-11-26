# 07 — TESTS PROMPT — Proste, deterministyczne smoke testy bez NuGet

Cel: Przygotuj minimalne, proste i deterministyczne testy „smoke” bez instalowania dodatkowych paczek NuGet (zgodnie z .clinerules). Testy mają tylko sprawdzić podstawowe działanie endpointów. Zakładamy, że w repo istnieją przykładowe dane jak w `08_data_prompt.md` (np. urządzenia o Id: `1`, `2`).

Podejście: Zamiast projektów testowych z frameworkami (xUnit/MSTest/NUnit), dostarcz prosty skrypt PowerShell, który:
- zakłada uruchomiony serwer API lokalnie,
- odpytuje podstawowe endpointy,
- weryfikuje kody odpowiedzi (200/404),
- kończy się kodem wyjścia 0 (sukces) lub 1 (błąd).

UWAGA: Nie uruchamiaj „dotnet run” z poziomu skryptu (aby uniknąć złożoności procesów). Najpierw uruchom API w oddzielnym terminalu:
- `dotnet run --project IotDemo/IotDemo.csproj --urls http://localhost:5050`
- potem w drugim terminalu uruchom skrypt smoke testów.

Wynik: Zwróć plik w następującym formacie (kompletny plik):
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagany plik i treść:

1) IotDemo.Tests/smoke-tests.ps1
- PowerShell 5+ / PowerShell 7 kompatybilny.
- Adres bazowy: `http://localhost:5050`
- Sprawdź kody odpowiedzi dla:
  - `GET /api/stats` → 200
  - `GET /api/devices` → 200
  - `GET /api/devices/1` → 200 (zakładając dane z 08_data_prompt)
  - `GET /api/devices/9999` → 404
  - `GET /api/devices/1/measurements` → 200
  - (opcjonalnie) `GET /api/locations/Warsaw/devices` → 200 dla danych z 08_data_prompt
- Gdy jakikolwiek test nie przejdzie, wypisz błąd i zakończ skrypt kodem 1.
- Jeśli wszystko OK, wypisz krótkie podsumowanie i zakończ kodem 0.

Przykładowa implementacja (zwróć dokładnie tę zawartość):

```
/// path: IotDemo.Tests/smoke-tests.ps1
/// --- file start ---
param(
    [string]$BaseUrl = "http://localhost:5050"
)

Write-Host "Smoke tests for IoTDemo at $BaseUrl"

function Test-StatusCode {
    param(
        [string]$Method,
        [string]$Url,
        [int]$Expected
    )
    try {
        $resp = Invoke-WebRequest -Method $Method -Uri $Url -UseBasicParsing -ErrorAction Stop
        $code = $resp.StatusCode
    } catch {
        if ($_.Exception.Response -ne $null) {
            try { $code = [int]$_.Exception.Response.StatusCode } catch { $code = -1 }
        } else {
            $code = -1
        }
    }

    if ($code -eq $Expected) {
        Write-Host "[OK] $Method $Url -> $code"
        return $true
    } else {
        Write-Error "[FAIL] $Method $Url -> got $code, expected $Expected"
        return $false
    }
}

$allOk = $true

$allOk = (Test-StatusCode -Method GET -Url "$BaseUrl/api/stats" -Expected 200) -and $allOk
$allOk = (Test-StatusCode -Method GET -Url "$BaseUrl/api/devices" -Expected 200) -and $allOk
$allOk = (Test-StatusCode -Method GET -Url "$BaseUrl/api/devices/1" -Expected 200) -and $allOk
$allOk = (Test-StatusCode -Method GET -Url "$BaseUrl/api/devices/9999" -Expected 404) -and $allOk
$allOk = (Test-StatusCode -Method GET -Url "$BaseUrl/api/devices/1/measurements" -Expected 200) -and $allOk
# Optional based on sample data (Warsaw location)
$allOk = (Test-StatusCode -Method GET -Url "$BaseUrl/api/locations/Warsaw/devices" -Expected 200) -and $allOk

if ($allOk) {
    Write-Host "All smoke tests passed."
    exit 0
} else {
    Write-Error "Some smoke tests failed."
    exit 1
}
/// --- file end ---
```

Instrukcja uruchomienia:
1) W terminalu A:
   - `dotnet run --project IotDemo/IotDemo.csproj --urls http://localhost:5050`
2) W terminalu B:
   - `pwsh ./IotDemo.Tests/smoke-tests.ps1` (PowerShell 7)
   - lub `powershell -ExecutionPolicy Bypass -File .\IotDemo.Tests\smoke-tests.ps1` (Windows PowerShell)

Zasady:
- Zero dodatkowych paczek NuGet.
- Testy krótkie, proste, deterministyczne.
- Brak mocków.
- Zależne od przykładowych danych z `08_data_prompt.md`.

Kryteria akceptacji:
- Skrypt istnieje i zwraca 0, gdy wszystkie podstawowe endpointy działają z przykładowymi danymi.
- W przypadku błędów zwraca 1 i czytelny komunikat.
