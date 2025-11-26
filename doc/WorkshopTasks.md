# Zadania warsztatowe – IoTDemo

Ten dokument opisuje zadania, które uczestnicy mają wykonać podczas pracy z projektem **IoTDemo**, najlepiej z pomocą LLM (Cline, Cursor, ChatGPT).


## Zadanie 1 – filtr maxBattery w GET /api/devices

### Cel

Dodać dodatkowy filtr `maxBattery` w endpointzie `GET /api/devices`.

### Wymagania

1. Dodać opcjonalny parametr query `maxBattery` (int).
2. Jeśli `maxBattery` jest podany, lista urządzeń powinna zawierać tylko te, które mają `BatteryPercent <= maxBattery`.

---

## Zadanie 2 – GET /api/locations/{location}/devices

### Cel

Dodać nowy endpoint:  
`GET /api/locations/{location}/devices`

### Wymagania

1. Endpoint ma zwracać urządzenia z danej lokalizacji (`Location`), przy czym porównanie jest case-insensitive.
2. Obsługuje paginację (`page`, `pageSize`) tak samo jak `GET /api/devices`.
3. Jeśli w danej lokalizacji nie ma żadnego urządzenia → zwrócić `404 Not Found`.
4. Rozszerzyć `ExternalDevicesLoader`, aby:
   - jeśli `Loc` w `DeviceRaw` jest puste lub `null`, ustawiać `Device.Location = "unknown"`,
   - zalogować ostrzeżenie (`ILogger`) w takiej sytuacji.


---

## Zadanie 3 – refaktor źródła danych + admin reload

### Cel

Dodać interfejs `IDevicesSource`, implementację `FileDevicesSource` oraz endpoint admina `POST /api/admin/reload-devices`.

### Wymagania

1. Wprowadzić interfejs `IDevicesSource`:

   ```csharp
   public interface IDevicesSource
   {
       IReadOnlyList<Device> LoadDevices();
   }
   ```

2. Dodać implementację `FileDevicesSource`, która:
   - używa `ExternalDevicesLoader`,
   - zwraca listę `Device`.

3. Przy starcie aplikacji:
   - dane urządzeń wczytywane są przez `IDevicesSource`,
   - przekazywane do `InMemoryDevicesRepository` poprzez `SetDevices(...)`.

4. Dodać endpoint:

   `POST /api/admin/reload-devices`

   - odczytuje `DeviceData:AllowDeviceReload` z konfiguracji,
   - jeśli `AllowDeviceReload == true`:
     - przeładowuje dane urządzeń przez `IDevicesSource`,
     - aktualizuje repozytorium `InMemoryDevicesRepository`,
     - zwraca `204 No Content` (lub `200 OK`),
   - jeśli `AllowDeviceReload == false`:
     - zwraca `400 Bad Request`,
     - nie zmienia danych w repozytorium.


---

## Sugestia promptów dla uczestników

Przykładowe prompt’y, które uczestnicy mogą użyć w LLM:

- „Dodaj obsługę parametru maxBattery do endpointu GET /api/devices.”
- „Dodaj endpoint GET /api/locations/{location}/devices z paginacją i zachowaniem 404 dla pustej lokalizacji.”
- „Zrefaktoryzuj ładowanie urządzeń tak, aby używać interfejsu IDevicesSource i dodaj endpoint POST /api/admin/reload-devices zgodnie z dokumentacją.”
