# 02 — EXTERNAL PROMPT — Modele Raw + Loadery (konfiguracja, mapowanie, prostota)

Cel: Utwórz warstwę `IotDemo.External` zgodnie z `doc/ExternalModels.md` i `.clinerules/projectRules.md`. Zaimplementuj dwa modele Raw oraz dwa loadery wczytujące dane z JSON, mapujące do modeli domenowych (`IotDemo.Domain`) z użyciem ścieżek z konfiguracji. Kod prosty, krótki, czytelny. Zero dodatkowych paczek.

Wynik: Zwróć pliki w poniższym formacie, każdy kompletny:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagane pliki i treści:

1) IotDemo/External/DeviceRaw.cs
- Przestrzeń nazw: `IotDemo.External`
- Klasa: `DeviceRaw` z właściwościami:
  - `int Id { get; set; }`
  - `string Device_Name { get; set; } = default!;`
  - `string Type { get; set; } = default!;`
  - `string? Loc { get; set; }`
  - `int Battery { get; set; }`
  - `string Fw_Ver { get; set; } = default!;`
  - `string Status { get; set; } = default!;`
  - `DateTime Last_Seen_Utc { get; set; }`

2) IotDemo/External/MeasurementRaw.cs
- Przestrzeń nazw: `IotDemo.External`
- Klasa: `MeasurementRaw` z właściwościami:
  - `string Id { get; set; } = default!;`
  - `string Device_Id { get; set; } = default!;`
  - `DateTime Ts_Utc { get; set; }`
  - `double? Temp_C { get; set; }`
  - `double? Humidity { get; set; }`

3) IotDemo/External/ExternalDevicesLoader.cs
- Przestrzeń nazw: `IotDemo.External`
- Zależności: `IotDemo.Domain`, `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Logging`, `System.Text.Json`
- Konstruktor: `ExternalDevicesLoader(ILogger<ExternalDevicesLoader> logger, IConfiguration configuration)`
  - Pobierz ścieżkę: `configuration["DeviceData:DevicesFilePath"] ?? "external-data/devices_raw.json"`
- Metoda: `public IReadOnlyList<Device> LoadDevices()`
  - Wczytaj plik JSON (prosty `File.ReadAllText`), zdeserializuj do `List<DeviceRaw>` (`JsonSerializer.Deserialize`).
  - Zamapuj do `List<Device>` wg zasad:
    - `Id`: `raw.Id.ToString()`
    - `Name`: `raw.Device_Name`
    - `Type` (case-insensitive): `"temperature"->TemperatureSensor`, `"humidity"->HumiditySensor`, `"multi"->MultiSensor`, `"gateway"->Gateway`, inne->`Other`
    - `Location`: `raw.Loc ?? ""`
    - `BatteryPercent`: `raw.Battery`
    - `FirmwareVersion`: `raw.Fw_Ver`
    - `Status` (case-insensitive): `"online"->Online`, `"offline"->Offline`, inne->`Unknown`
    - `LastSeenUtc`: `raw.Last_Seen_Utc`
  - Obsłuż błędy: try/catch, zaloguj `logger.LogError(ex, "...")`, zwróć pustą listę przy błędzie.
  - Zwróć `IReadOnlyList<Device>`.

4) IotDemo/External/ExternalMeasurementsLoader.cs
- Przestrzeń nazw: `IotDemo.External`
- Zależności: `IotDemo.Domain`, `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Logging`, `System.Text.Json`
- Konstruktor: `ExternalMeasurementsLoader(ILogger<ExternalMeasurementsLoader> logger, IConfiguration configuration)`
  - Pobierz ścieżkę: `configuration["MeasurementData:MeasurementsFilePath"] ?? "external-data/measurements_raw.json"`
- Metoda: `public IReadOnlyList<Measurement> LoadMeasurements()`
  - Wczytaj JSON do `List<MeasurementRaw>`.
  - Mapowanie:
    - `Id`: `raw.Id`
    - `DeviceId`: `raw.Device_Id`
    - `TimestampUtc`: `raw.Ts_Utc`
    - `TemperatureC`: `raw.Temp_C`
    - `HumidityPercent`: `raw.Humidity`
  - Obsłuż błędy: try/catch + logowanie, zwróć pustą listę.
  - Zwróć `IReadOnlyList<Measurement>`.

Zasady:
- Tylko `System.Text.Json` i wbudowane klasy .NET. Zero nowych paczek.
- Ścieżki zawsze z konfiguracji (zgodnie z `doc/Configuration.md`).
- Loadery nie zwracają modeli Raw – tylko domenowe.
- Kod krótki, metody krótkie, nazwy jasne. Bez nadmiarowych helperów.
- W `LoadDevices()` oraz `LoadMeasurements()` zwracaj pustą listę w przypadku błędu, aby API pozostało stabilne.

Kryteria akceptacji:
- Pliki, przestrzenie nazw i własności dokładnie jak powyżej.
- Mapowanie zgodne z `doc/ExternalModels.md`.
- Wczytywanie ścieżek z `appsettings*.json`.
- Brak użycia modeli Raw poza loaderami.
- Prosty try/catch z logowaniem i pustą listą na błędzie.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
