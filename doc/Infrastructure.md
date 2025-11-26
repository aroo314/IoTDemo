# Infrastruktura – IoTDemo

Ten dokument opisuje warstwę **Infrastructure** projektu IoTDemo. Odpowiada ona za:
- przechowywanie danych w pamięci,
- dostarczanie danych z plików (za pomocą źródeł danych),
- abstrakcję nad dostępem do urządzeń i pomiarów.

Wszystkie klasy z tej warstwy znajdują się w przestrzeni nazw:

`IotDemo.Infrastructure`

---

## InMemoryDevicesRepository

Przechowuje urządzenia IoT w pamięci aplikacji.

**Wymagania:**
- repozytorium musi oferować dostęp do listy urządzeń,
- lista może być zastępowana przy przeładowaniu danych (`reload-devices`),
- repozytorium nie może samodzielnie wczytywać plików.

Szkielet:

```
public class InMemoryDevicesRepository
{
    private List<Device> _devices = new();

    public IReadOnlyList<Device> GetAll() => _devices;

    public void SetDevices(IEnumerable<Device> devices)
    {
        _devices = devices.ToList();
    }

    public Device? GetById(string id)
    {
        return _devices.FirstOrDefault(d => d.Id == id);
    }
}
```

---

## InMemoryMeasurementsRepository

Przechowuje pomiary w pamięci.

```
public class InMemoryMeasurementsRepository
{
    private List<Measurement> _measurements = new();

    public IReadOnlyList<Measurement> GetByDeviceId(string deviceId)
    {
        return _measurements
            .Where(m => m.DeviceId == deviceId)
            .OrderByDescending(m => m.TimestampUtc)
            .ToList();
    }

    public void SetMeasurements(IEnumerable<Measurement> measurements)
    {
        _measurements = measurements.ToList();
    }
}
```

---

## IDevicesSource

Abstrakcja źródła danych urządzeń.

```
public interface IDevicesSource
{
    IReadOnlyList<Device> LoadDevices();
}
```

---

## FileDevicesSource

Implementacja `IDevicesSource` oparta o loadery z przestrzeni `IotDemo.External`.

**Zadania:**
- odczytać ścieżkę do pliku z konfiguracji,
- użyć `ExternalDevicesLoader` do wczytania danych,
- zwrócić urządzenia jako modele domenowe.

Szkielet:

```
public class FileDevicesSource : IDevicesSource
{
    private readonly ExternalDevicesLoader _loader;

    public FileDevicesSource(ExternalDevicesLoader loader)
    {
        _loader = loader;
    }

    public IReadOnlyList<Device> LoadDevices()
    {
        return _loader.LoadDevices();
    }
}
```

---

## Zasady ogólne

1. Repozytoria **nie mogą** samodzielnie odczytywać plików.
2. Źródła danych (`IDevicesSource`) muszą korzystać z loaderów z katalogu `External`.
3. Repozytoria są jedynym runtime źródłem danych dla endpointów.
4. Endpoint admina (`POST /api/admin/reload-devices`) powinien:
   - wywołać metodę `LoadDevices()` na `IDevicesSource`,
   - zastąpić dane w `InMemoryDevicesRepository`,
   - respektować wartość `AllowDeviceReload` w konfiguracji.
