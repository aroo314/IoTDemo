# Endpointy API – IoTDemo

Ten dokument opisuje wszystkie endpointy API dostępne w projekcie **IoTDemo**.

Domyślny prefiks:  
`/api`

## 1. GET /api/devices

Zwraca listę urządzeń IoT z możliwością filtrowania i paginacji.

### Query parameters (opcjonalne)

- `type` – `temperature`, `humidity`, `multi`, `gateway`, `other`
- `status` – `online`, `offline`, `unknown`
- `location` – string, filtr po lokalizacji (case-insensitive)
- `minBattery` – minimalny poziom baterii (int)
- `maxBattery` – maksymalny poziom baterii (int) – dodawany w zadaniu warsztatowym
- `page` – numer strony, domyślnie `1`
- `pageSize` – rozmiar strony, domyślnie `10`

### Odpowiedzi

- `200 OK` – lista urządzeń w formie paginowanej

## 2. GET /api/devices/{id}

Zwraca szczegóły pojedynczego urządzenia.

### Route parameters

- `id` – identyfikator urządzenia (np. `dev-001`)

### Query parameters (opcjonalne)

- `includeLastMeasurement` – `true` / `false` (domyślnie `false`)

### Odpowiedzi

- `200 OK` – urządzenie znalezione
- `404 Not Found` – urządzenie nie istnieje

## 3. GET /api/devices/{id}/measurements

Zwraca listę pomiarów dla konkretnego urządzenia.

### Route parameters

- `id` – identyfikator urządzenia

### Query parameters (opcjonalne)

- `from` – data/czas od (ISO 8601, UTC)
- `to` – data/czas do (ISO 8601, UTC)
- `limit` – maksymalna liczba rekordów, domyślnie `100`
- `order` – `asc` lub `desc` (domyślnie `desc`)

### Odpowiedzi

- `200 OK` – urządzenie istnieje, zwrócono listę (może być pusta)
- `404 Not Found` – urządzenie nie istnieje

## 4. GET /api/stats

Zwraca statystyki dotyczące urządzeń i pomiarów.

### Odpowiedzi

- `200 OK` – statystyki w postaci JSON (liczba urządzeń, online/offline, podział po typach, itp.)

## 5. POST /api/admin/reload-devices

Endpoint administracyjny do przeładowania danych urządzeń z pliku.

Zachowanie:
- Sprawdza wartość `DeviceData:AllowDeviceReload` z konfiguracji.
- Jeśli `true`:
  - używa `IDevicesSource` do wczytania danych,
  - zastępuje dane w `InMemoryDevicesRepository`,
  - zwraca `204 No Content` lub `200 OK`.
- Jeśli `false`:
  - zwraca `400 Bad Request`.

## 6. GET /api/locations/{location}/devices (zadanie warsztatowe)

Endpoint dodawany w ramach zadania:

- filtruje urządzenia po `Location` (case-insensitive),
- wspiera paginację (`page`, `pageSize`),
- jeśli brak urządzeń w danej lokalizacji → `404 Not Found`,
- jeśli są urządzenia → `200 OK` z listą urządzeń jak w `GET /api/devices`.

## Zasady ogólne

1. Endpointy rejestrowane w `Program.cs` przez metody z katalogu `Endpoints/`.  
2. Wszystkie endpointy mają być udokumentowane w Swaggerze (`SwaggerConfig.cs`).  
3. Endpointy korzystają z modeli domenowych (`IotDemo.Domain`) oraz repozytoriów/infrastruktury.
4. Odpowiedzi mogą być modelami domenowymi lub prostymi DTO (jeśli udokumentowane); nigdy modele Raw.
