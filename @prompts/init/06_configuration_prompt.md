# 06 — CONFIGURATION PROMPT — appsettings.json + appsettings.Development.json

Cel: Utwórz pliki konfiguracyjne zgodne z `doc/Configuration.md`. Loadery w `External/` mają zawsze pobierać ścieżki z konfiguracji; endpoint admina ma respektować `AllowDeviceReload`. Kod prosty, bez dodatków.

Wynik: Zwróć pliki w poniższym formacie, każdy kompletny:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagane pliki i treści (ściśle):

1) IotDemo/appsettings.json
- Sekcje:
  - `DeviceData`:
    - `DevicesFilePath`: `"external-data/devices_raw.json"`
    - `AllowDeviceReload`: `true`
  - `MeasurementData`:
    - `MeasurementsFilePath`: `"external-data/measurements_raw.json"`
  - `Logging`:
    - `LogLevel`: `Default="Information"`, `Microsoft.AspNetCore="Warning"`
  - `AllowedHosts`: `"*"`

Przykład (zwróć dokładnie tę zawartość):
```
/// path: IotDemo/appsettings.json
/// --- file start ---
{
  "DeviceData": {
    "DevicesFilePath": "external-data/devices_raw.json",
    "AllowDeviceReload": true
  },
  "MeasurementData": {
    "MeasurementsFilePath": "external-data/measurements_raw.json"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
/// --- file end ---
```

2) IotDemo/appsettings.Development.json
- Nadpisuje wybrane ustawienia w trybie deweloperskim (np. bardziej szczegółowe logowanie).
- Utrzymuj prostotę; nie zmieniaj nazw sekcji.

Przykład:
```
/// path: IotDemo/appsettings.Development.json
/// --- file start ---
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "DeviceData": {
    "AllowDeviceReload": true
  }
}
/// --- file end ---
```

Zasady:
- Nie zmieniaj nazw sekcji konfiguracyjnych.
- Loadery **nie mogą** trzymać ścieżek na sztywno — zawsze pobierają z konfiguracji.
- Endpoint `POST /api/admin/reload-devices` sprawdza `DeviceData:AllowDeviceReload`.
- Konfiguracja jest wstrzykiwana przez DI w `Program.cs`.

Kryteria akceptacji:
- Oba pliki istnieją z zawartością jak powyżej.
- Ścieżki do plików zgodne z folderem `external-data`.
- Minimalne i czytelne ustawienia logowania.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
