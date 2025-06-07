Projekt CarSharingSystem to nowoczesna aplikacja webowa umożliwiająca użytkownikom wynajem pojazdów w modelu współdzielenia. System został zaprojektowany w architekturze mikroserwisowej, z myślą o skalowalności, elastyczności i łatwości utrzymania.

Technologie i architektura
.NET 9 Web API
Backend systemu został stworzony z wykorzystaniem .NET 9 oraz ASP.NET Core Web API. Odpowiada on za logikę biznesową, uwierzytelnianie, zarządzanie użytkownikami, pojazdami, rezerwacjami oraz płatnościami.

Blazor (WebAssembly)
Warstwa frontendowa została zbudowana przy użyciu Blazora w modelu WebAssembly. Pozwala to na uruchamianie aplikacji po stronie klienta z pełną integracją z Web API oraz zapewnia dynamiczny, responsywny interfejs użytkownika.

xUnit
W celu zapewnienia wysokiej jakości kodu i niezawodności systemu, testy jednostkowe oraz testy integracyjne zostały zaimplementowane przy użyciu biblioteki xUnit. Testowane są kluczowe komponenty logiki biznesowej oraz punktów końcowych API.

Architektura mikroserwisowa
System składa się z kilku niezależnych mikroserwisów odpowiedzialnych m.in. za zarządzanie użytkownikami, flotą pojazdów, rezerwacjami, płatnościami oraz powiadomieniami. Ułatwia to rozwój i wdrażanie poszczególnych komponentów bez wpływu na cały system.

Azure i hosting w chmurze
Wszystkie mikroserwisy oraz frontend są hostowane w chmurze Microsoft Azure. Wykorzystywane są usługi takie jak Azure App Services, Azure SQL Database, Azure Functions oraz Azure Service Bus w celu zapewnienia wydajności i niezawodności działania systemu.

Główne funkcjonalności
Rejestracja i logowanie użytkowników

Przegląd dostępnych pojazdów i lokalizacji

Rezerwacja i wypożyczanie pojazdów

Historia rezerwacji i płatności

Obsługa powiadomień i przypomnień

Cel projektu
Celem projektu jest stworzenie bezpiecznego, skalowalnego i łatwego w utrzymaniu systemu carsharingowego, który może zostać wdrożony w środowisku produkcyjnym z wykorzystaniem nowoczesnych technologii Microsoftu.
