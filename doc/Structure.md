# Struktura projektu – IoTDemo

Ten dokument opisuje pełną, obowiązującą strukturę projektu **IoTDemo**.  
Każdy LLM oraz każdy developer pracujący z repozytorium powinien się jej trzymać.

Struktura jest zoptymalizowana pod:
- warsztat 1,5h,
- pracę z LLM (Cursor, Cline, ChatGPT),
- Minimal API + .NET 8,
- przejrzystość i łatwą nawigację.

## Struktura katalogów i plików

```
.
├─ .gitignore
├─ IotDemo.sln
├─ doc/
│  ├─ Configuration.md
│  ├─ DomainModels.md
│  ├─ Endpoints.md
│  ├─ ExternalModels.md
│  ├─ Goal.md
│  ├─ Infrastructure.md
│  ├─ InputData.md
│  ├─ Structure.md
│  └─ WorkshopTasks.md
├─ IotDemo.Tests/
│  └─ .gitkeep
└─ IotDemo/
   ├─ appsettings.Development.json
   ├─ appsettings.json
   ├─ IotDemo.csproj
   ├─ IotDemo.csproj.user
   ├─ Program.cs
   ├─ Properties/
   │  └─ launchSettings.json
   │
   ├─ External/
   │  ├─ DeviceRaw.cs
   │  ├─ MeasurementRaw.cs
   │  ├─ ExternalDevicesLoader.cs
   │  └─ ExternalMeasurementsLoader.cs
   │
   ├─ Domain/
   │  ├─ Device.cs
   │  ├─ DeviceStatus.cs
   │  ├─ DeviceType.cs
   │  └─ Measurement.cs
   │
   ├─ Infrastructure/
   │  ├─ InMemoryDevicesRepository.cs
   │  ├─ InMemoryMeasurementsRepository.cs
   │  ├─ IDevicesSource.cs
   │  └─ FileDevicesSource.cs
   │
   ├─ Endpoints/
   │  ├─ DevicesEndpoints.cs
   │  ├─ MeasurementsEndpoints.cs
   │  ├─ StatsEndpoints.cs
   │  └─ AdminEndpoints.cs
   │
   └─ Swagger/
      └─ SwaggerConfig.cs
```

## Wytyczne

- **doc/** – pełna dokumentacja projektu (ten plik oraz pozostałe opisy).
- **IotDemo/** – główny projekt API:
  - `Program.cs` – konfiguracja aplikacji, DI, rejestracja endpointów,
  - `appsettings*.json` – konfiguracja,
  - pozostałe katalogi: `External/`, `Domain/`, `Infrastructure/`, `Endpoints/`, `Swagger/`.

- **IotDemo.Tests/** – folder testów jednostkowych (pusty, zawiera `.gitkeep`).

## Zasady dla LLM

1. Nie zmieniać struktury folderów bez wyraźnego polecenia.
2. Nowe pliki tworzyć w odpowiednich katalogach (np. nowe endpointy → `Endpoints/`).
3. Nie mieszać modeli domenowych (`Domain/`) z modelami Raw (`External/`).
4. Repozytoria (`Infrastructure/`) nie powinny czytać plików – tym zajmują się loadery (`External/`) i źródła danych.
5. Odpowiedzi API mogą być modelami domenowymi lub prostymi DTO (jeśli udokumentowane); nigdy modele Raw.
