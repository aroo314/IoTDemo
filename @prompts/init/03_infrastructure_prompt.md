# 03 — INFRASTRUCTURE PROMPT — Repozytoria i źródła danych (InMemory + File)

Cel: Utwórz warstwę `IotDemo.Infrastructure` zgodnie z `doc/Infrastructure.md` i zasadami `.clinerules/projectRules.md`. Repozytoria przechowują dane w pamięci. Czytanie plików odbywa się wyłącznie przez loadery z `IotDemo.External`, a dostęp realizuje `IDevicesSource`. Kod prosty, krótki, czytelny.

Wynik: Zwróć pliki w poniższym formacie, każdy kompletny:
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```

Wymagane pliki i treści:

1) IotDemo/Infrastructure/InMemoryDevicesRepository.cs
- Przestrzeń nazw: `IotDemo.Infrastructure`
- Typ: public class `InMemoryDevicesRepository`
- Wewnętrznie: `private List<Device> _devices = new();`
- Metody:
  - `public IReadOnlyList<Device> GetAll() => _devices;`
  - `public void SetDevices(IEnumerable<Device> devices) { _devices = devices.ToList(); }`
  - `public Device? GetById(string id) { return _devices.FirstOrDefault(d => d.Id == id); }`
- Użyj `IotDemo.Domain` dla typu `Device`.
- Zero operacji I/O w tym repozytorium.

2) IotDemo/Infrastructure/InMemoryMeasurementsRepository.cs
- Przestrzeń nazw: `IotDemo.Infrastructure`
- Typ: public class `InMemoryMeasurementsRepository`
- Wewnętrznie: `private List<Measurement> _measurements = new();`
- Metody:
  - `public IReadOnlyList<Measurement> GetByDeviceId(string deviceId)`
    - filtr po `DeviceId`
    - sortuj malejąco po `TimestampUtc`
    - zwróć `.ToList()`
  - `public void SetMeasurements(IEnumerable<Measurement> measurements) { _measurements = measurements.ToList(); }`
- Użyj `IotDemo.Domain` dla typu `Measurement`.

3) IotDemo/Infrastructure/IDevicesSource.cs
- Przestrzeń nazw: `IotDemo.Infrastructure`
- public interface `IDevicesSource`
  - `IReadOnlyList<Device> LoadDevices();`
- Interfejs nie zawiera logiki, tylko podpis metody.
- Użyj `IotDemo.Domain` dla typu `Device`.

4) IotDemo/Infrastructure/FileDevicesSource.cs
- Przestrzeń nazw: `IotDemo.Infrastructure`
- public class `FileDevicesSource` implementująca `IDevicesSource`
- Konstruktor: przyjmuje `IotDemo.External.ExternalDevicesLoader loader` i przypisuje do pola prywatnego.
- `public IReadOnlyList<Device> LoadDevices()` wywołuje i zwraca `loader.LoadDevices()`.
- Zero dodatkowych operacji I/O, zero mapowań (robi to loader).

Zasady:
- Repozytoria nie czytają plików.
- Źródła danych (`IDevicesSource`) korzystają z loaderów z `External`.
- Proste, krótkie klasy i metody; brak zbędnych helperów, brak abstrakcji ponad wymagane.
- Wyłącznie modele domenowe w publicznych API repozytoriów/źródeł.

Kryteria akceptacji:
- Pliki, przestrzenie nazw i metody dokładnie jak powyżej oraz zgodnie z `doc/Infrastructure.md`.
- `InMemoryDevicesRepository` i `InMemoryMeasurementsRepository` działają na listach w pamięci.
- `IDevicesSource` definiuje pojedynczą metodę `LoadDevices()`.
- `FileDevicesSource` deleguje do `ExternalDevicesLoader`, bez I/O i bez mapowania.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
