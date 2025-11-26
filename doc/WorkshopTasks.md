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




// TREŚĆ zadania
1. Twoim zadaniem jest stworzenie promptu, który pozwoli Ci szybko i skutecznie zrozumieć dowolny projekt.
Przygotuj prompt, który po wklejeniu do AI pozwoli Ci uzyskać odpowiedzi na pytania:
Co robi aplikacja?
Jak jest zbudowana (technologie, architektura, integracje)?
Jakie są jej główne funkcje oraz najważniejsze zadania, które realizuje?



                                                                     //PROMPTY
                                                                     1. *„Mam wejść w nowy projekt i potrzebuję pełnego zrozumienia aplikacji. Opisz proszę w sposób jasny i możliwie zwięzły:

                                                                     Co dokładnie robi aplikacja i jaki problem rozwiązuje?

                                                                     Jak jest zbudowana — jakie technologie, architektura, integracje i główne komponenty wykorzystuje?

                                                                     Jakie są jej kluczowe funkcje oraz najważniejsze zadania, które realizuje?

                                                                     Jeśli są istotne zależności, ograniczenia lub obecne wyzwania w projekcie — dodaj je również.”*

                                                                     //
                                                                     2. *„Chcę szybko zrozumieć projekt. Wyjaśnij proszę:

                                                                     Jaki jest cel aplikacji i jakie problemy użytkowników rozwiązuje?

                                                                     W jakich technologiach została zbudowana, z jakiej architektury korzysta i z jakimi systemami się integruje?

                                                                     Jakie funkcje są kluczowe i jakie zadania biznesowe lub techniczne realizuje aplikacja na co dzień?
                                                                     Dodaj też najważniejsze ograniczenia, punkty ryzyka i aktualny status prac.”*

                                                                     //
                                                                     3. Potrzebuję szybkiego obrazu projektu. Opisz:

                                                                     Do czego służy aplikacja?

                                                                     Jak jest zbudowana (technologie, architektura, integracje)?

                                                                     Jakie są jej główne funkcje i najważniejsze zadania?
                                                                     Podaj tylko najistotniejsze informacje.”*



2.  Twoim zadaniem jest dodać możliwość filtrowania listy urządzeń po nazwie.

                                                                     1. Dodaj do endpointu pobierającego listę Device możliwość filtrowania po nazwie.
                                                                     Jeśli parametr name jest podany, zwróć tylko urządzenia, których nazwa zawiera ten tekst (case-insensitive).
                                                                     Pokaż gotowy kod zmodyfikowanego endpointu.



                                                                     2.Potrzebuję dodać filtr po nazwie dla listy urządzeń (Device) w moim API.
                                                                     Proszę:

                                                                     zmodyfikuj endpoint zwracający listę urządzeń tak, aby przyjmował opcjonalny parametr name,

                                                                     zaimplementuj filtrowanie tak, by zwracane były tylko te rekordy, których Device.Name zawiera wartość name (bez rozróżniania wielkości liter),

                                                                     pokaż kompletny kod metody endpointu oraz zapytania (LINQ / SQL / repozytorium),

                                                                     dodaj przykładowe wywołanie endpointu z filtrem, np. GET /api/devices?name=thermo.
                                                                                                                                          
                                                                     3. Mam projekt z encją Device oraz endpointem zwracającym listę urządzeń.
                                                                     Twoim zadaniem jest zmodyfikować istniejący kod tak, aby dodać możliwość filtrowania listy urządzeń po nazwie.
                                                                     Zrób to w następujący sposób:

                                                                     dodaj opcjonalny parametr name w endpointzie / metodzie pobierającej listę urządzeń,

                                                                     jeśli name jest podane, zwróć tylko te urządzenia, których nazwa zawiera podany fragment (filtrowanie case-insensitive),

                                                                     pokaż zmiany w kontrolerze / endpointzie oraz w miejscu, gdzie wykonywane jest zapytanie do bazy (np. repozytorium lub DbContext).
                                                                     Pokaż gotowy kod przed i po zmianie oraz krótki komentarz, co zostało zrobione.

3.



                                                                     