# 04 — ENDPOINTS PROMPT — Implementacja Minimal API (/api)

Cel: Utwórz endpointy zgodne z `doc/Endpoints.md` w przestrzeni nazw `IotDemo.Endpoints`. Każdy endpoint ma być krótki, jednozadaniowy. Zwracaj modele domenowe lub proste DTO (lokalne `record` w pliku endpointu). Nigdy modele Raw. Kod prosty, krótki, czytelny. Zero dodatkowych paczek.

Wynik: Zwróć pliki w poniższym formacie, każdy kompletny:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagane pliki i ich zakres:

1) IotDemo/Endpoints/DevicesEndpoints.cs
- Przestrzeń nazw: `IotDemo.Endpoints`
- public static class `DevicesEndpoints`
- public static void `Map(RouteGroupBuilder api)` — rejestruje wszystkie endpointy dot. urządzeń
- Wstrzykiwanie zależności przez parametry handlerów (Minimal API): `InMemoryDevicesRepository`, `InMemoryMeasurementsRepository`
- Implementacje:
  - `GET /api/devices` — filtrowanie i paginacja
    - Query (opcjonalne): 
      - `type`: `temperature|humidity|multi|gateway|other` (case-insensitive)
      - `status`: `online|offline|unknown` (case-insensitive)
      - `location`: string, case-insensitive equals
      - `minBattery`: int?
      - `maxBattery`: int?
      - `search`: fragment nazwy (case-insensitive contains)
      - `page`: int, domyślnie 1
      - `pageSize`: int, domyślnie 10
    - Źródło: `InMemoryDevicesRepository.GetAll()`
    - Mapowanie `type` -> `DeviceType` oraz `status` -> `DeviceStatus` wewnątrz metody (proste funkcje lokalne)
    - Paginacja: subset po `Skip((page-1)*pageSize).Take(pageSize)`
    - Zwraca `200 OK` z listą urządzeń (modele domenowe)
  - `GET /api/devices/{id}` — szczegóły urządzenia
    - Route: `{id}` (string)
    - Query (opcjonalne): `includeLastMeasurement`: bool (domyślnie false)
    - Jeśli urządzenie nie istnieje -> `404 Not Found`
    - Jeśli `includeLastMeasurement == true`:
      - Zwróć DTO jako lokalny `record DeviceWithLastMeasurement(Device device, Measurement? lastMeasurement)`
      - `lastMeasurement` to najnowszy pomiar dla danego urządzenia (Repository po DeviceId, FirstOrDefault po TimestampUtc desc)
      - `200 OK`
    - W przeciwnym razie `200 OK` z samym `Device`
  - `GET /api/locations/{location}/devices` — warsztatowy
    - Route: `{location}` (string)
    - Query (opcjonalne): `page` (domyślnie 1), `pageSize` (domyślnie 10)
    - Filtr: `device.Location` equals `location` case-insensitive
    - Jeśli brak wyników -> `404 Not Found`
    - Inaczej `200 OK` z paginowaną listą

2) IotDemo/Endpoints/MeasurementsEndpoints.cs
- Przestrzeń nazw: `IotDemo.Endpoints`
- public static class `MeasurementsEndpoints`
- public static void `Map(RouteGroupBuilder api)`
- Wstrzykiwanie: `InMemoryDevicesRepository`, `InMemoryMeasurementsRepository`
- Implementacja:
  - `GET /api/devices/{id}/measurements`
    - Route: `{id}`
    - Query (opcjonalne):
      - `from`: ISO 8601, UTC (DateTime) — użyj `DateTime.TryParse` (prosto)
      - `to`: ISO 8601, UTC (DateTime)
      - `limit`: int, domyślnie `100`
      - `order`: `asc|desc` (domyślnie `desc`)
    - Jeśli urządzenie nie istnieje -> `404 Not Found`
    - Pobierz pomiary repo: `GetByDeviceId(id)`
    - Filtrowanie po zakresie dat (jeśli podane)
    - Sortowanie wg `TimestampUtc` (respect `order`)
    - Zastosuj `Take(limit)`
    - Zwróć `200 OK` (lista może być pusta)

3) IotDemo/Endpoints/StatsEndpoints.cs
- Przestrzeń nazw: `IotDemo.Endpoints`
- public static class `StatsEndpoints`
- public static void `Map(RouteGroupBuilder api)`
- Wstrzykiwanie: `InMemoryDevicesRepository`, `InMemoryMeasurementsRepository`
- Implementacja:
  - `GET /api/stats`
    - Zwraca prosty DTO (lokalny `record`) np.:
      ```
      public record StatsDto(
        int TotalDevices,
        int Online,
        int Offline,
        int Unknown,
        Dictionary<string,int> ByType
      );
      ```
    - `ByType`: liczba urządzeń dla każdego typu (`TemperatureSensor`, `HumiditySensor`, `MultiSensor`, `Gateway`, `Other`)
    - `200 OK`

4) IotDemo/Endpoints/AdminEndpoints.cs
- Przestrzeń nazw: `IotDemo.Endpoints`
- public static class `AdminEndpoints`
- public static void `Map(RouteGroupBuilder api)`
- Wstrzykiwanie: `IDevicesSource`, `InMemoryDevicesRepository`, `IConfiguration`
- Implementacja:
  - `POST /api/admin/reload-devices`
    - Sprawdź `DeviceData:AllowDeviceReload` z `IConfiguration` (bool)
    - Jeśli `false` -> `400 Bad Request` (np. z krótką wiadomością `"Reload disabled"`)
    - Jeśli `true`:
      - Wywołaj `IDevicesSource.LoadDevices()`
      - Podmień dane w repo: `SetDevices(...)`
      - Zwróć `204 No Content`

Zasady:
- Prefiks `/api` zapewnia `Program.cs` przez `app.MapGroup("/api")`; metody `Map` przyjmują `RouteGroupBuilder api`.
- Wszystkie endpointy krótkie, jednozadaniowe.
- Brak custom middleware, brak filtrów.
- Zwracaj: 404 — brak zasobu, 400 — błędne/odrzucone żądanie, 200/204 — sukces.
- Case-insensitive dla `location`, `search`, `type`, `status`.
- Tylko modele domenowe lub lokalne DTO w endpointach (recordy).
- Nigdy nie używaj modeli Raw.
- Komentarze wyłącznie krótkie przy definicji lokalnych DTO (cel opisu).

Kryteria akceptacji:
- Pliki, klasy i metody dokładnie jak powyżej.
- Zachowanie 1:1 zgodne z `doc/Endpoints.md`.
- Prosty, czytelny kod Minimal API z DI przez parametry handlerów.
- Brak zbędnych klas i helperów.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
