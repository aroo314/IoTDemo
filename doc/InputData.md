# Dane wejściowe – IoTDemo

Ten dokument opisuje dane wejściowe używane przez projekt **IoTDemo**.  
Dane są przechowywane w plikach JSON w katalogu głównym repozytorium:

`external-data/`

## Plik devices_raw.json

Surowe dane urządzeń IoT:

```json
[
  {
    "id": "dev-001",
    "device_name": "SmartToaster v2.0",
    "type": "sensor_temp",
    "loc": "kitchen",
    "battery": 87,
    "fw_ver": "1.2.3",
    "status": "online",
    "last_seen_utc": "2025-11-23T09:15:00Z"
  },
  {
    "id": "dev-002",
    "device_name": "Basement Humidity Sensor",
    "type": "sensor_humidity",
    "loc": "basement",
    "battery": 42,
    "fw_ver": "0.9.1-beta",
    "status": "offline",
    "last_seen_utc": "2025-11-20T21:00:00Z"
  }
]
```

## Plik measurements_raw.json

Surowe dane pomiarów:

```json
[
  {
    "id": "m-1001",
    "device_id": "dev-001",
    "ts_utc": "2025-11-23T09:10:00Z",
    "temp_c": 180.5,
    "humidity": null
  },
  {
    "id": "m-1002",
    "device_id": "dev-002",
    "ts_utc": "2025-11-23T09:05:00Z",
    "temp_c": 14.2,
    "humidity": 65.1
  }
]
```

## Zasady

1. Dane wejściowe służą tylko jako źródło demo – można je dowolnie rozszerzać o kolejne urządzenia i pomiary.
2. Loader (`ExternalDevicesLoader`, `ExternalMeasurementsLoader`) musi korzystać ze ścieżek z konfiguracji (`DeviceData`, `MeasurementData`).
3. Format JSON może być rozszerzany, ale istniejące pola nie powinny być usuwane, aby nie łamać mapowania.
