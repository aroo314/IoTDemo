# Cline Rules – IoTDemo

Zasady, których model LLM musi przestrzegać podczas pracy nad projektem IoTDemo.

## Styl kodu
- Kod ma być **prosty**, **krótki**, **czytelny**.
- Unikaj skomplikowanych wzorców, nadmiernej abstrakcji, polimorfizmu i „magii”.
- Preferuj jasne nazwy, krótkie metody i minimalną liczbę klas.
- Minimal API jest obowiązujące.

## Struktura projektu
- Nowe endpointy → `IotDemo/Endpoints/`
- Modele domenowe → `IotDemo/Domain/`
- Modele Raw + loadery → `IotDemo/External/`
- Repozytoria + źródła danych → `IotDemo/Infrastructure/`
- Dokumentacja → `doc/`
- **Nie dodawaj nowych folderów ani warstw**, chyba że zostanie to wyraźnie polecone.

## Zasady endpointów
- Każdy endpoint ma być krótki i jednozadaniowy.
- Zwracaj modele domenowe (nigdy Raw).
- Obsługa błędów:
  - `404` – brak zasobu,
  - `400` – błędne dane,
  - `200` / `204` – sukces.
- Zero custom middleware, zero filtrów — projekt ma być lekki.

## Refaktoring
- Refaktoruj tylko tam, gdzie proszę.
- Nie zmieniaj nazw plików ani folderów.
- Zawsze zachowuj zgodność z dokumentacją w `doc/`.

## Testy
- Testy mają być krótkie, proste, deterministyczne.
- Bez mocków jeśli nie są konieczne.

## Ważne
- Nie instaluj paczek NuGet bez wyraźnej prośby.
- Nie generuj zbędnych klas, pluginów ani „ulepszeń”.
- Wszystkie zmiany muszą być spójne z resztą projektu.

## Komunikacja
- Jeśli zadanie jest niejasne — zapytaj.
- Po wygenerowaniu kodu pokaż krótki opis zmian (max 2–3 zdania).

## Cel
**Generuj kod: prosty, krótki, czytelny i zgodny ze strukturą projektu.**