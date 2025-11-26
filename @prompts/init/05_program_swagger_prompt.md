# 05 — PROGRAM + SWAGGER PROMPT — Minimal API bootstrap i Swagger

Cel: Utwórz pliki `Program.cs` oraz `Swagger/SwaggerConfig.cs` zgodnie z zasadami Minimal API i dokumentacją (`doc/Configuration.md`, `doc/Endpoints.md`). Rejestruj wszystkie zależności przez DI, wczytaj dane początkowe, zgrupuj endpointy pod `/api`, skonfiguruj prosty Swagger.

Wynik: Zwróć pliki w poniższym formacie, każdy kompletny:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagane pliki i treści:

1) IotDemo/Program.cs
- Minimal API, .NET 8, prosty i czytelny plik startowy.
- Rejestracja DI:
  - `AddSingleton<InMemoryDevicesRepository>()`
  - `AddSingleton<InMemoryMeasurementsRepository>()`
  - `AddSingleton<ExternalDevicesLoader>()`
  - `AddSingleton<ExternalMeasurementsLoader>()`
  - `AddSingleton<IDevicesSource, FileDevicesSource>()`
- Inicjalne wczytanie danych (prosto w Program.cs przy starcie):
  - Użyj zarejestrowanych loaderów:
    - `ExternalDevicesLoader.LoadDevices()` -> ustaw na `InMemoryDevicesRepository.SetDevices(...)`
    - `ExternalMeasurementsLoader.LoadMeasurements()` -> ustaw na `InMemoryMeasurementsRepository.SetMeasurements(...)`
  - Obsługa błędów jest już w loaderach (zwracają pustą listę), więc tutaj bez try/catch.
- Swagger:
  - Użyj `SwaggerConfig.AddSwagger(builder.Services)` i potem `SwaggerConfig.UseSwagger(app)`
- Grupowanie endpointów:
  - `var api = app.MapGroup("/api");`
  - Wywołaj metody Map:
    - `DevicesEndpoints.Map(api);`
    - `MeasurementsEndpoints.Map(api);`
    - `StatsEndpoints.Map(api);`
    - `AdminEndpoints.Map(api);`
- `app.Run();`

Przykładowa treść (dostosuj `using` do przestrzeni nazw w repo):
```
/// path: IotDemo/Program.cs
/// --- file start ---
using IotDemo.Domain;
using IotDemo.Endpoints;
using IotDemo.External;
using IotDemo.Infrastructure;
using IotDemo.Swagger;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddSingleton<InMemoryDevicesRepository>();
builder.Services.AddSingleton<InMemoryMeasurementsRepository>();
builder.Services.AddSingleton<ExternalDevicesLoader>();
builder.Services.AddSingleton<ExternalMeasurementsLoader>();
builder.Services.AddSingleton<IDevicesSource, FileDevicesSource>();

// Swagger
SwaggerConfig.AddSwagger(builder.Services);

var app = builder.Build();

// Swagger UI
SwaggerConfig.UseSwagger(app);

// Wczytanie danych początkowych
var devicesRepo = app.Services.GetRequiredService<InMemoryDevicesRepository>();
var measurementsRepo = app.Services.GetRequiredService<InMemoryMeasurementsRepository>();
var devicesLoader = app.Services.GetRequiredService<ExternalDevicesLoader>();
var measurementsLoader = app.Services.GetRequiredService<ExternalMeasurementsLoader>();

devicesRepo.SetDevices(devicesLoader.LoadDevices());
measurementsRepo.SetMeasurements(measurementsLoader.LoadMeasurements());

// Grupowanie endpointów
var api = app.MapGroup("/api");

// Rejestracja endpointów
DevicesEndpoints.Map(api);
MeasurementsEndpoints.Map(api);
StatsEndpoints.Map(api);
AdminEndpoints.Map(api);

app.Run();
/// --- file end ---
```

2) IotDemo/Swagger/SwaggerConfig.cs
- Prosta, lekka konfiguracja OpenAPI, bez nadmiaru.
- Udostępnij dwie metody statyczne:
  - `AddSwagger(IServiceCollection services)` – wywołuje `AddEndpointsApiExplorer()` i `AddSwaggerGen()`.
  - `UseSwagger(WebApplication app)` – wywołuje `app.UseSwagger()` i `app.UseSwaggerUI()`.
- Przestrzeń nazw: `IotDemo.Swagger`.

Przykładowa treść:
```
/// path: IotDemo/Swagger/SwaggerConfig.cs
/// --- file start ---
namespace IotDemo.Swagger;

public static class SwaggerConfig
{
    public static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void UseSwagger(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
/// --- file end ---
```

Zasady:
- Minimal API, brak dodatkowych paczek.
- Prosty `Program.cs` — tylko DI, wczytanie danych, grupowanie `/api`, Swagger, uruchomienie.
- Brak custom middleware/filtrów.
- Zależności zgodne z poprzednimi promptami (Domain/External/Infrastructure/Endpoints/Swagger).

Kryteria akceptacji:
- Program.cs i SwaggerConfig.cs kompilowalne i zgodne z .NET 8 Minimal API.
- Wszystkie endpointy rejestrowane przez `.Map(api)` i prefiks `/api`.
- Swagger widoczny i działa (UI i definicje endpointów).
- Dane inicjalne wczytane do repozytoriów przy starcie aplikacji.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
