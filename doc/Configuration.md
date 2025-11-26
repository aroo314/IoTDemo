# Konfiguracja projektu – IoTDemo

Ten dokument opisuje wszystkie ustawienia konfiguracyjne projektu **IoTDemo**, używane w plikach `appsettings.json` i `appsettings.Development.json`.

## Pliki konfiguracyjne

Projekt korzysta z dwóch głównych plików konfiguracyjnych:

- `IotDemo/appsettings.json`  
- `IotDemo/appsettings.Development.json`

Plik Development nadpisuje ustawienia tylko w środowisku developerskim.

# Sekcje konfiguracji

## 1. DeviceData

Ustawienia dotyczące urządzeń IoT.

```
"DeviceData": {
  "DevicesFilePath": "external-data/devices_raw.json",
  "AllowDeviceReload": true
}
```

**DevicesFilePath**  
Ścieżka do pliku `devices_raw.json`.  
Używana przez `ExternalDevicesLoader`.

**AllowDeviceReload**  
- `true` — endpoint `POST /api/admin/reload-devices` może przeładować dane  
- `false` — endpoint zwraca `400 Bad Request`

---

## 2. MeasurementData

Ustawienia dotyczące pomiarów.

```
"MeasurementData": {
  "MeasurementsFilePath": "external-data/measurements_raw.json"
}
```

**MeasurementsFilePath**  
Ścieżka do pliku `measurements_raw.json`.  
Używana przez `ExternalMeasurementsLoader`.

---

## 3. Logging

Konfiguracja logowania ASP.NET Core:

```
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

---

## 4. AllowedHosts

```
"AllowedHosts": "*"
```

# Przykład pełnego appsettings.json

```
{
  "DeviceData": {
    "DevicesFilePath": "external-data/devices_raw.json",
    "AllowDeviceReload": true
  },
  "MeasurementData": {
    "MeasurementsFilePath": "external-data/measurements_raw.json"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

# Zasady

1. Nie zmieniać nazw sekcji konfiguracyjnych.  
2. Loadery **nie mogą** mieć ścieżek na sztywno — zawsze używać konfiguracji.  
3. Endpoint `POST /api/admin/reload-devices` musi sprawdzać `AllowDeviceReload`.  
4. Konfiguracja jest wstrzykiwana przez DI w `Program.cs`.
