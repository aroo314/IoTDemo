# Cel projektu – IoTDemo

Projekt **IoTDemo** jest materiałem warsztatowym na sesję około 1,5 godziny dla inżynierów o różnym poziomie seniority.

## Główne założenia

- Temat: **IoT Devices API** – proste API do obsługi urządzeń IoT i ich pomiarów.
- Technologia: **.NET (preferowany .NET 8)**, **ASP.NET Core Minimal API**.
- Dane:
  - przechowywane w pamięci (in-memory),
  - ładowane z plików JSON przez warstwę `External/`.
- Brak prawdziwej bazy danych – ma być **lekko, szybko i prosto do zrozumienia**.

## Cele techniczne

1. Pokazać, jak używać LLM (np. Cline, Cursor, ChatGPT) do:
   - dodawania nowych endpointów,
   - refaktoringu kodu,
   - generowania testów jednostkowych,
   - pracy z konfiguracją i warstwami aplikacji.
2. Zaprezentować czytelną, spójną strukturę projektu:
   - `External/` – modele Raw i loadery,
   - `Domain/` – modele domenowe,
   - `Infrastructure/` – repozytoria i źródła danych,
   - `Endpoints/` – definicje endpointów,
   - `Swagger/` – konfiguracja OpenAPI.

## Cele warsztatowe

1. Uczestnik potrafi:
   - rozumieć istniejący kod,
   - poprosić LLM o dodanie nowego feature’a,
   - wygenerować testy do istniejącego endpointu,
   - bezpiecznie refaktoryzować kod z pomocą LLM.
2. Repozytorium służy jako **source of truth**:
   - dokumentacja w folderze `doc/`,
   - prosty, jednolity styl projektowy,
   - pełna przejrzystość logiki i punktów rozszerzeń.

## Kontekst

- Projekt ma charakter **demo/warsztatowy**, nie produkcyjny.
- Kod może być celowo uproszczony, aby:
  - ułatwić zrozumienie,
  - zostawić miejsce na zadania warsztatowe,
  - być wygodnym poligonem do pracy z LLM.
