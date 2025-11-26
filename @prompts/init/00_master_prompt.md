# MASTER PROMPT — Wygeneruj kompletną aplikację IoTDemo (.NET 8 Minimal API)

Cel: Wygeneruj od zera lekkie API „IoTDemo” zgodne z dokumentacją w `doc/` oraz zasadami w `.clinerules/projectRules.md`. Kod ma być prosty, krótki, czytelny. Minimal API obowiązkowe. Bez dodatkowych paczek NuGet.

Wynik: Zwróć kompletny zestaw plików projektu w zadeklarowanej strukturze. Każdy plik MUSI być podany w formacie:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```
Bez komentarzy między plikami, bez pomijania fragmentów. Każdy plik kompletny.

Wymagana struktura (ściśle):
```
.
├─ IotDemo.sln
├─ IotDemo/
│  ├─ appsettings.json
│  ├─ appsettings.Development.json
│  ├─ IotDemo.csproj
│  ├─ Program.cs
│  ├─ Properties/
│  │  └─ launchSettings.json
│  ├─ External/
│  │  ├─ DeviceRaw.cs
│  │  ├─ MeasurementRaw.cs
│  │  ├─ ExternalDevicesLoader.cs
│  │  └─ ExternalMeasurementsLoader.cs
│  ├─ Domain/
│  │  ├─ Device.cs
│  │  ├─ DeviceStatus.cs
│  │  ├─ DeviceType.cs
│  │  └─ Measurement.cs
│  ├─ Infrastructure/
│  │  ├─ InMemoryDevicesRepository.cs
│  │  ├─ InMemoryMeasurementsRepository.cs
│  │  ├─ IDevicesSource.cs
│  │  └─ FileDevicesSource.cs
│  ├─ Endpoints/
│  │  ├─ DevicesEndpoints.cs
│  │  ├─ MeasurementsEndpoints.cs
│  │  ├─ StatsEndpoints.cs
│  │  └─ AdminEndpoints.cs
│  └─ Swagger/
│     └─ SwaggerConfig.cs
└─ external-data/
   ├─ devices_raw.json
   └─ measurements_raw.json
```

Zasady globalne:
- Minimal API, .NET 8.
- Brak nowych folderów/warstw poza wymienionymi.
- Endpointy jednozadaniowe, proste. Zwracaj modele domenowe lub proste DTO (jeśli jawnie wprowadzone w kodzie i udokumentowane komentarzem). Nigdy modele Raw.
- Obsługa błędów: 404 – brak zasobu, 400 – błędne dane/odrzucone żądanie, 200/204 – sukces.
- Loadery (`External/*`) zawsze biorą ścieżki z konfiguracji (`appsettings*.json`).
- Repozytoria (`Infrastructure/*`) nigdy nie czytają plików – używają źródeł danych / loaderów.
- Swagger lekki, czytelny, bez nadmiaru.

Zakres implementacji (co ma znaleźć się w plikach):

1) Domain (`IotDemo.Domain`)
- `DeviceStatus` enum: Online/Offline/Unknown.
- `DeviceType` enum: TemperatureSensor/HumiditySensor/MultiSensor/Gateway/Other.
- `Device` class: Id (string), Name, Type, Location, BatteryPercent (int), FirmwareVersion, Status, LastSeenUtc (DateTime).
- `Measurement` class: Id, DeviceId, TimestampUtc (DateTime), TemperatureC (double?), HumidityPercent (double?).

2) External (`IotDemo.External`)
- `DeviceRaw`: jak w dokumentacji (int Id, Device_Name, Type, Loc?, Battery, Fw_Ver, Status, Last_Seen_Utc).
- `MeasurementRaw`: jak w dokumentacji (Id, Device_Id, Ts_Utc, Temp_C?, Humidity?).
- `ExternalDevicesLoader`:
  - W konstruktorze pobierz `DeviceData:DevicesFilePath` (domyślnie `external-data/devices_raw.json`).
  - `LoadDevices()`:
    - Wczytaj JSON (System.Text.Json), zdeserializuj do `List<DeviceRaw>`.
    - Zamapuj na `List<Device>`:
      - `Id`: `raw.Id.ToString()`.
      - `Name`: `raw.Device_Name`.
      - `Type`: mapowanie case-insensitive: `"temperature"->TemperatureSensor`, `"humidity"->HumiditySensor`, `"multi"->MultiSensor`, `"gateway"->Gateway`, inne->Other.
      - `Location`: `raw.Loc ?? ""`.
      - `BatteryPercent`: `raw.Battery`.
      - `FirmwareVersion`: `raw.Fw_Ver`.
      - `Status`: `"online"->Online`, `"offline"->Offline`, inne->Unknown (case-insensitive).
      - `LastSeenUtc`: `raw.Last_Seen_Utc`.
    - Zwróć `IReadOnlyList<Device>`.
- `ExternalMeasurementsLoader`:
  - W konstruktorze pobierz `MeasurementData:MeasurementsFilePath` (domyślnie `external-data/measurements_raw.json`).
  - `LoadMeasurements()`:
    - Wczytaj JSON do `List<MeasurementRaw>`.
    - Mapuj do `List<Measurement>`:
      - `Id`: `raw.Id`.
      - `DeviceId`: `raw.Device_Id`.
      - `TimestampUtc`: `raw.Ts_Utc`.
      - `TemperatureC`: `raw.Temp_C`.
      - `HumidityPercent`: `raw.Humidity`.
    - Zwróć `IReadOnlyList<Measurement>`.

3) Infrastructure (`IotDemo.Infrastructure`)
- `InMemoryDevicesRepository`:
  - Przechowywanie listy `Device`.
  - Metody: `GetAll()`, `SetDevices(IEnumerable<Device>)`, `GetById(string id)`.
- `InMemoryMeasurementsRepository`:
  - Przechowywanie listy `Measurement`.
  - Metody: `GetByDeviceId(string deviceId)` (posortowane malejąco po `TimestampUtc`), `SetMeasurements(IEnumerable<Measurement>)`.
- `IDevicesSource`:
  - `IReadOnlyList<Device> LoadDevices();`
- `FileDevicesSource`:
  - Wstrzyknięty `ExternalDevicesLoader`.
  - `LoadDevices()` zwraca dane z `ExternalDevicesLoader.LoadDevices()`.

4) Endpoints (`IotDemo.Endpoints`) — prefiks `/api`
- `DevicesEndpoints`:
  - `GET /api/devices` — filtrowanie i paginacja:
    - Query: `type` (`temperature|humidity|multi|gateway|other`), `status` (`online|offline|unknown`), `location` (case-insensitive), `minBattery` (int), `maxBattery` (int), `search` (fragment nazwy, case-insensitive), `page` (domyślnie 1), `pageSize` (domyślnie 10).
    - Zwraca listę urządzeń po filtrach i wycięciu strony (prosta paginacja przez subset listy).
  - `GET /api/devices/{id}` — szczegóły:
    - Query: `includeLastMeasurement` (bool, domyślnie false).
    - Jeśli `includeLastMeasurement==true`, zwróć prosty DTO z polami `device` i `lastMeasurement` (rekord/typ zdefiniowany lokalnie w pliku endpointu, udokumentowany komentarzem). W innym przypadku zwróć sam `Device`.
    - `404` gdy brak urządzenia.
- `MeasurementsEndpoints`:
  - `GET /api/devices/{id}/measurements`:
    - Query: `from` (ISO 8601, UTC), `to` (ISO 8601, UTC), `limit` (domyślnie 100), `order` (`asc|desc`, domyślnie `desc`).
    - `404` jeśli brak urządzenia.
    - Zwraca listę (może być pusta) po filtrach i sortowaniu.
- `StatsEndpoints`:
  - `GET /api/stats`:
    - Zwraca prosty DTO statystyk (zdefiniowany lokalnie): total urządzeń, online/offline/unknown, podział po typach.
- `AdminEndpoints`:
  - `POST /api/admin/reload-devices`:
    - Sprawdza `DeviceData:AllowDeviceReload`.
    - Jeśli `true`: wywołuje `IDevicesSource.LoadDevices()`, podmienia dane w `InMemoryDevicesRepository`, zwraca `204 No Content`.
    - Jeśli `false`: `400 Bad Request`.

- `GET /api/locations/{location}/devices` (w ramach `DevicesEndpoints` lub osobny endpoint):
  - Filtruj po `Location` (case-insensitive), paginuj (`page`, `pageSize`).
  - `404` jeśli brak urządzeń w lokalizacji; inaczej `200` z listą.

5) Swagger (`IotDemo.Swagger`)
- `SwaggerConfig.cs` — prosta konfiguracja OpenAPI:
  - `AddEndpointsApiExplorer()`, `AddSwaggerGen()` w Program.cs.
  - Udokumentuj krótko endpointy i parametry (summary/description).

6) Program (`IotDemo/Program.cs`)
- Minimal API:
  - `var builder = WebApplication.CreateBuilder(args);`
  - Rejestracja DI:
    - `AddSingleton<InMemoryDevicesRepository>()`
    - `AddSingleton<InMemoryMeasurementsRepository>()`
    - `AddSingleton<ExternalDevicesLoader>()`
    - `AddSingleton<ExternalMeasurementsLoader>()`
    - `AddSingleton<IDevicesSource, FileDevicesSource>()`
  - Wczytanie inicjalne danych przy starcie:
    - Załaduj urządzenia i pomiary przez loadery/źródła i ustaw w repozytoriach.
  - Swagger: `AddEndpointsApiExplorer`, `AddSwaggerGen`, w `app`: `UseSwagger()`, `UseSwaggerUI()`.
  - Grupowanie endpointów: `var api = app.MapGroup("/api");` i wywołanie metod rejestrujących endpointy z klas w `Endpoints/*`.
  - `app.Run();`

7) Konfiguracja (`IotDemo/appsettings*.json`)
- Zgodnie z `doc/Configuration.md`:
  - `DeviceData` (`DevicesFilePath`, `AllowDeviceReload`)
  - `MeasurementData` (`MeasurementsFilePath`)
  - `Logging`, `AllowedHosts`
- `appsettings.Development.json` nadpisuje wybrane sekcje w dev.

8) Dane przykładowe (`external-data/*.json`)
- Przygotuj minimalne przykłady zgodne z `DeviceRaw` i `MeasurementRaw` (po kilka rekordów), aby API po starcie było użyteczne.

Wytyczne implementacyjne:
- Filtrowanie case-insensitive dla `location` i `search`.
- Bez zbędnej abstrakcji. Krótkie metody, proste nazwy.
- Nie używaj modeli Raw poza warstwą `External`.
- DTO (jeśli potrzebne) twórz jako lokalne `record` w plikach endpointów i opisz komentarzem.
- Nie instaluj dodatkowych paczek NuGet.
- Użyj `System.Text.Json` do JSON, prosty try/catch z logowaniem w loaderach.

Kryteria akceptacji (automatyczne):
- Struktura katalogów jak wyżej.
- Endpointy z `doc/Endpoints.md` działają, kody odpowiedzi zgodne.
- Modele domenowe zgodne z `doc/DomainModels.md`.
- Loadery i konfiguracja zgodne z `doc/ExternalModels.md` i `doc/Configuration.md`.
- Infra zgodna z `doc/Infrastructure.md`.
- Zero modeli Raw w odpowiedziach API.
- Styl: prosty, krótki, czytelny.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
/// path: IotDemo.sln — klasyczne rozwiązanie Visual Studio dla pojedynczego projektu `IotDemo`.
