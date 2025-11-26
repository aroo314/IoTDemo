# Modele zewnętrzne – IoTDemo

Ten dokument opisuje **modele zewnętrzne (Raw)** oraz **loadery** używane w projekcie **IoTDemo**.  
Służą one do wczytywania „brzydkich” danych z plików JSON i mapowania ich na modele domenowe z przestrzeni nazw `IotDemo.Domain`.

Wszystkie typy w tej warstwie znajdują się w przestrzeni nazw:

`IotDemo.External`

## DeviceRaw

```csharp
namespace IotDemo.External;

public class DeviceRaw
{
    public string Id { get; set; } = default!;
    public string Device_Name { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string? Loc { get; set; }
    public int Battery { get; set; }
    public string Fw_Ver { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime Last_Seen_Utc { get; set; }
}
```

## MeasurementRaw

```csharp
namespace IotDemo.External;

public class MeasurementRaw
{
    public string Id { get; set; } = default!;
    public string Device_Id { get; set; } = default!;
    public DateTime Ts_Utc { get; set; }
    public double? Temp_C { get; set; }
    public double? Humidity { get; set; }
}
```

## ExternalDevicesLoader

Szkielet:

```csharp
using IotDemo.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IotDemo.External;

public class ExternalDevicesLoader
{
    private readonly ILogger<ExternalDevicesLoader> _logger;
    private readonly string _filePath;

    public ExternalDevicesLoader(ILogger<ExternalDevicesLoader> logger, IConfiguration configuration)
    {
        _logger = logger;
        _filePath = configuration["DeviceData:DevicesFilePath"] 
                    ?? "external-data/devices_raw.json";
    }

    public IReadOnlyList<Device> LoadDevices()
    {
        // Wczytanie pliku, deserializacja do List<DeviceRaw>, mapowanie do List<Device>.
        throw new NotImplementedException("Implement in code.");
    }
}
```

## ExternalMeasurementsLoader

Szkielet:

```csharp
using IotDemo.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IotDemo.External;

public class ExternalMeasurementsLoader
{
    private readonly ILogger<ExternalMeasurementsLoader> _logger;
    private readonly string _filePath;

    public ExternalMeasurementsLoader(ILogger<ExternalMeasurementsLoader> logger, IConfiguration configuration)
    {
        _logger = logger;
        _filePath = configuration["MeasurementData:MeasurementsFilePath"] 
                    ?? "external-data/measurements_raw.json";
    }

    public IReadOnlyList<Measurement> LoadMeasurements()
    {
        // Wczytanie pliku, deserializacja do List<MeasurementRaw>, mapowanie do List<Measurement>.
        throw new NotImplementedException("Implement in code.");
    }
}
```

## Zasady

1. Modele Raw nie są używane bezpośrednio przez endpointy.  
2. Loadery korzystają ze ścieżek z konfiguracji.  
3. Endpointy i repozytoria pracują wyłącznie na modelach domenowych.
