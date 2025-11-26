# 08 — DATA PROMPT — Przykładowe dane JSON (external-data/*.json)

Cel: Utwórz minimalne dane przykładowe w katalogu `external-data/` zgodne z `doc/ExternalModels.md`. Dane mają wspierać uruchomienie API oraz smoke testy z `07_tests_prompt.md` (np. urządzenie o Id `1` w lokalizacji `Warsaw`).

Wynik: Zwróć pliki w poniższym formacie, każdy kompletny:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagane pliki i treści:

1) external-data/devices_raw.json
- Struktura zgodna z `IotDemo.External.DeviceRaw`:
  - `Id` (int), `Device_Name` (string), `Type` (string), `Loc` (string?), `Battery` (int),
    `Fw_Ver` (string), `Status` (string), `Last_Seen_Utc` (ISO 8601, UTC)
- Wartości `Type`: `temperature`, `humidity`, `multi`, `gateway`, `other` (case-insensitive mapowane do `DeviceType`)
- Wartości `Status`: `online`, `offline`, inne → `Unknown` (case-insensitive)
- Przykładowa zawartość:
```
/// path: external-data/devices_raw.json
/// --- file start ---
[
  {
    "Id": 1,
    "Device_Name": "Temp Sensor A",
    "Type": "temperature",
    "Loc": "Warsaw",
    "Battery": 87,
    "Fw_Ver": "1.2.0",
    "Status": "online",
    "Last_Seen_Utc": "2025-11-26T18:00:00Z"
  },
  {
    "Id": 2,
    "Device_Name": "Humidity Sensor B",
    "Type": "humidity",
    "Loc": "Krakow",
    "Battery": 64,
    "Fw_Ver": "1.0.3",
    "Status": "offline",
    "Last_Seen_Utc": "2025-11-25T21:30:00Z"
  },
  {
    "Id": 3,
    "Device_Name": "Gateway C",
    "Type": "gateway",
    "Loc": "Warsaw",
    "Battery": 100,
    "Fw_Ver": "2.0.0",
    "Status": "online",
    "Last_Seen_Utc": "2025-11-26T18:05:00Z"
  }
]
/// --- file end ---
```

2) external-data/measurements_raw.json
- Struktura zgodna z `IotDemo.External.MeasurementRaw`:
  - `Id` (string), `Device_Id` (string), `Ts_Utc` (ISO 8601, UTC),
    `Temp_C` (double?), `Humidity` (double?)
- `Device_Id` musi odpowiadać `Device.Id` po mapowaniu (`Id` z `DeviceRaw` konwertowany do stringa)
- Przykładowa zawartość:
```
/// path: external-data/measurements_raw.json
/// --- file start ---
[
  {
    "Id": "m-001",
    "Device_Id": "1",
    "Ts_Utc": "2025-11-26T17:55:00Z",
    "Temp_C": 22.4,
    "Humidity": null
  },
  {
    "Id": "m-002",
    "Device_Id": "1",
    "Ts_Utc": "2025-11-26T18:05:00Z",
    "Temp_C": 22.9,
    "Humidity": null
  },
  {
    "Id": "m-003",
    "Device_Id": "2",
    "Ts_Utc": "2025-11-25T21:20:00Z",
    "Temp_C": null,
    "Humidity": 48.5
  },
  {
    "Id": "m-004",
    "Device_Id": "2",
    "Ts_Utc": "2025-11-25T21:28:00Z",
    "Temp_C": null,
    "Humidity": 47.8
  },
  {
    "Id": "m-005",
    "Device_Id": "3",
    "Ts_Utc": "2025-11-26T18:04:00Z",
    "Temp_C": null,
    "Humidity": null
  }
]
/// --- file end ---
```

Zasady:
- Ścieżki muszą odpowiadać konfiguracji (`DeviceData:DevicesFilePath`, `MeasurementData:MeasurementsFilePath`).
- Dane mają być małe, czytelne i deterministyczne.
- Lokacje powinny umożliwiać test warsztatowy `GET /api/locations/{location}/devices` (np. `Warsaw`).

Kryteria akceptacji:
- Oba pliki istnieją i spełniają schematy `DeviceRaw` oraz `MeasurementRaw`.
- Po starcie API endpointy działają na tych danych: `/api/devices`, `/api/devices/1`, `/api/devices/1/measurements`, `/api/stats`, `/api/locations/Warsaw/devices`.
- Smoke test z `IotDemo.Tests/smoke-tests.ps1` przechodzi.

Instrukcja uruchomienia:
1) `dotnet run --project IotDemo/IotDemo.csproj --urls http://localhost:5050`
