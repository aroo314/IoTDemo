# 01 — DOMAIN PROMPT — Utwórz modele domenowe IoTDemo (.NET 8)

Cel: Wygeneruj komplet modeli domenowych zgodnych z `doc/DomainModels.md` w przestrzeni nazw `IotDemo.Domain`. Kod ma być prosty, krótki, czytelny. Bez dodatkowych klas, atrybutów, adnotacji – tylko to, co poniżej.

Wynik: Zwróć pliki dokładnie w poniższym formacie i ścieżkach (każdy plik kompletny):
```
/// path: <RELATYWNA_ŚCIEŻKA_OD_KORZENIA_REPO>
/// --- file start ---
<CAŁA TREŚĆ PLIKU>
/// --- file end ---
```
Bez komentarzy między plikami i bez pomijania fragmentów.

Wymagane pliki i treści (ściśle):

1) IotDemo/Domain/DeviceStatus.cs
- Przestrzeń nazw: `IotDemo.Domain`
- Enum: `DeviceStatus` z wartościami: `Online`, `Offline`, `Unknown`.

2) IotDemo/Domain/DeviceType.cs
- Przestrzeń nazw: `IotDemo.Domain`
- Enum: `DeviceType` z wartościami: `TemperatureSensor`, `HumiditySensor`, `MultiSensor`, `Gateway`, `Other`.

3) IotDemo/Domain/Device.cs
- Przestrzeń nazw: `IotDemo.Domain`
- Klasa: `Device`
- Właściwości z publicznymi get/set:
  - `string Id { get; set; } = default!;`
  - `string Name { get; set; } = default!;`
  - `DeviceType Type { get; set; }`
  - `string Location { get; set; } = default!;`
  - `int BatteryPercent { get; set; }`
  - `string FirmwareVersion { get; set; } = default!;`
  - `DeviceStatus Status { get; set; }`
  - `DateTime LastSeenUtc { get; set; }`

4) IotDemo/Domain/Measurement.cs
- Przestrzeń nazw: `IotDemo.Domain`
- Klasa: `Measurement`
- Właściwości z publicznymi get/set:
  - `string Id { get; set; } = default!;`
  - `string DeviceId { get; set; } = default!;`
  - `DateTime TimestampUtc { get; set; }`
  - `double? TemperatureC { get; set; }`
  - `double? HumidityPercent { get; set; }`

Zasady:
- Tylko przestrzeń nazw `IotDemo.Domain`.
- Brak zależności od `IotDemo.External`.
- Zero zbędnej abstrakcji, komentarzy i atrybutów. Krótkie, proste definicje.

Kryteria akceptacji:
- Pliki i nazwy dokładnie jak wyżej.
- Zawartość 1:1 zgodna z `doc/DomainModels.md`.
- Kompilowalne definicje modeli bez dodatkowego kodu.

Na koniec zwróć WSZYSTKIE pliki w zadanym formacie.
