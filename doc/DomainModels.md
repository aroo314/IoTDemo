# Modele domenowe – IoTDemo

Ten dokument opisuje wszystkie **modele domenowe** używane w projekcie **IoTDemo**.  
Preferowane do zwracania przez API oraz przechowywania w repozytoriach; proste DTO są dopuszczalne, jeśli jawnie wprowadzone i udokumentowane.

## Przestrzeń nazw

Wszystkie modele domenowe powinny znajdować się w przestrzeni nazw:

`IotDemo.Domain`

## DeviceStatus

Reprezentuje status urządzenia IoT.

```csharp
namespace IotDemo.Domain;

public enum DeviceStatus
{
    Online,
    Offline,
    Unknown
}
```

## DeviceType

Reprezentuje typ urządzenia IoT.

```csharp
namespace IotDemo.Domain;

public enum DeviceType
{
    TemperatureSensor,
    HumiditySensor,
    MultiSensor,
    Gateway,
    Other
}
```

## Device

Reprezentuje urządzenie IoT w systemie.

```csharp
namespace IotDemo.Domain;

public class Device
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DeviceType Type { get; set; }
    public string Location { get; set; } = default!;
    public int BatteryPercent { get; set; }
    public string FirmwareVersion { get; set; } = default!;
    public DeviceStatus Status { get; set; }
    public DateTime LastSeenUtc { get; set; }
}
```

## Measurement

Reprezentuje pojedynczy pomiar z urządzenia IoT.

```csharp
namespace IotDemo.Domain;

public class Measurement
{
    public string Id { get; set; } = default!;
    public string DeviceId { get; set; } = default!;
    public DateTime TimestampUtc { get; set; }
    public double? TemperatureC { get; set; }
    public double? HumidityPercent { get; set; }
}
```

## Zasady

1. Modele domenowe nie powinny zależeć od modeli z przestrzeni nazw `IotDemo.External`.  
2. Endpointy API powinny zwracać dane na podstawie tych modeli (lub prostych DTO, jeśli zostaną wprowadzone).  
3. Zmiany w modelach domenowych powinny być odzwierciedlane w dokumentacji.
