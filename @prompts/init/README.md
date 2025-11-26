# Prompty do wygenerowania aplikacji IoTDemo (.NET 8 Minimal API)

Cel: Zestaw gotowych promptów, które pozwolą LLM (np. Cline, Cursor, ChatGPT) utworzyć kompletną aplikację IoTDemo zgodnie z dokumentacją i zasadami projektu.

Aplikacja: IoT Devices API – lekka, czytelna, warsztatowa implementacja Minimal API na .NET 8.

Źródła wymagań: `doc/Structure.md`, `doc/Goal.md`, `doc/Endpoints.md`, `doc/DomainModels.md`, `doc/ExternalModels.md`, `doc/Infrastructure.md`, `doc/Configuration.md` oraz `.clinerules/projectRules.md`.

## Jak używać

- Każdy plik poniżej to osobny, konkretny prompt do wklejenia w LLM.
- Wykonuj je w kolejności od `00_master_prompt.md`, potem kolejne kroki 01–06 (opcjonalnie 07–08).
- Po każdym kroku uruchom budowanie i sprawdź działanie (np. `dotnet run`) – iteruj krótkimi, jednozadaniowymi zmianami.

## Zasady, które MUSZĄ być spełnione

- Kod ma być prosty, krótki, czytelny. Minimal API obowiązkowe.
- Nie dodawaj nowych folderów ani warstw poza: `IotDemo/External`, `IotDemo/Domain`, `IotDemo/Infrastructure`, `IotDemo/Endpoints`, `IotDemo/Swagger`, `doc/`.
- Endpointy:
  - krótki, jednozadaniowy kod,
  - zwracaj modele domenowe (nigdy Raw),
  - kody odpowiedzi: 404 brak zasobu, 400 błędne dane/odrzucone żądanie, 200/204 sukces,
  - bez custom middleware i filtrów.
- Brak instalacji nowych paczek NuGet (chyba że wyraźnie poproszone).
- Loadery w `External/` zawsze używają ścieżek z konfiguracji (z `appsettings*.json`).
- Repozytoria w `Infrastructure/` nie czytają plików – korzystają ze źródeł danych i loaderów.
- Swagger jest skonfigurowany lekko i jasno (bez nadmiarowych dodatków).

## Kolekcja promptów

- 00_master_prompt.md – główny prompt generatora: tworzy strukturę projektu, pliki, rejestrację DI, Swagger, Minimal API oraz podstawowe endpointy.
- 01_domain_prompt.md – tworzenie modeli domenowych w `IotDemo/Domain` (Device, Measurement, DeviceStatus, DeviceType).
- 02_external_prompt.md – modele Raw i loadery w `IotDemo/External` (DeviceRaw, MeasurementRaw, ExternalDevicesLoader, ExternalMeasurementsLoader).
- 03_infrastructure_prompt.md – repozytoria/źródła danych w `IotDemo/Infrastructure` (InMemoryDevicesRepository, InMemoryMeasurementsRepository, IDevicesSource, FileDevicesSource).
- 04_endpoints_prompt.md – implementacja endpointów z `doc/Endpoints.md` (devices, device by id, measurements, stats, admin reload oraz warsztatowy locations).
- 05_program_swagger_prompt.md – `Program.cs` (DI, rejestracja endpointów) i `Swagger/SwaggerConfig.cs` (OpenAPI).
- 06_configuration_prompt.md – `appsettings.json` i `appsettings.Development.json` zgodnie z `doc/Configuration.md`.
- 07_tests_prompt.md (opcjonalnie) – proste, krótkie testy deterministyczne dla podstawowych scenariuszy.
- 08_data_prompt.md (opcjonalnie) – przygotowanie przykładowych danych w `external-data/*.json`.

## Kryteria akceptacji

- Zgodność ze strukturą z `doc/Structure.md`.
- Zgodność endpointów z `doc/Endpoints.md`, w tym:
  - GET /api/devices (filtrowanie + paginacja),
  - GET /api/devices/{id} (+ opcjonalny includeLastMeasurement),
  - GET /api/devices/{id}/measurements (zakres dat, limit, kolejność),
  - GET /api/stats,
  - POST /api/admin/reload-devices (respektuje `DeviceData:AllowDeviceReload`),
  - GET /api/locations/{location}/devices (warsztatowy).
- Modele domenowe zgodnie z `doc/DomainModels.md`.
- Modele Raw + loadery zgodnie z `doc/ExternalModels.md` (konfiguracja ścieżek!).
- Infrastruktura zgodnie z `doc/Infrastructure.md`.
- Konfiguracja zgodnie z `doc/Configuration.md`.
- Styl i zasady z `.clinerules/projectRules.md`.

## Szybki start

1) Wklej `00_master_prompt.md` do LLM i uruchom generowanie.
2) Następnie 01–06 w kolejności; po każdym kroku zbuduj projekt (`dotnet build`) i uruchom (`dotnet run`).
3) Zweryfikuj w Swaggerze, czy wszystkie endpointy działają zgodnie z opisem.
4) Dodaj opcjonalnie 07–08, jeśli potrzebne.

## Uwagi warsztatowe

- Proś o małe, iteracyjne zmiany (jednozadaniowe).
- Pilnuj prostoty i spójności – to nie jest projekt produkcyjny, tylko demo.
- Wszystkie modyfikacje muszą być zgodne z dokumentacją w `doc/` oraz z regułami w `.clinerules/projectRules.md`.
