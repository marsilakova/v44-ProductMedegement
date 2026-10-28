Product Management System (Week 44)

En robust och strukturerad konsolapplikation i C# för dynamisk produkthantering, utvecklad enligt principerna för Objektorienterad Programmering (OOP). Projektet uppfyller alla krav för Level 3.

A robust and structured C# console application for dynamic product management, built following Object-Oriented Programming (OOP) principles. This project fully satisfies all requirements up to Level 3.

───

✨ Funktioner / Features

• Dynamic Data Entry: Sömlös inmatning av produktkategori, produktnamn och pris med direkt avbrott via 'q' (Input loop with instant exit using 'q').
• Advanced Error Handling: Fullständig validering av inmatning som förhindrar tomma fält, felaktiga prisformat samt negativa värden med hjälp av decimal.TryParse.
• LINQ Integration:
◦ OrderBy() för automatisk sortering från lägsta till högsta pris.
◦ Sum() för exakt beräkning av totalsumman av alla produkter.
◦ Where() för flexibel sökning efter produkter baserat på namn eller kategori.
◦ FirstOrDefault() för kontroll av dubbletter i systemet.
• Null Safety: Implementerad C# Null-safety (required egenskaper, ? operatorer och ?? "") för att säkerställa 0 Warnings och fullständig stabilitet vid framtida databasintegration.

───

🛠️ Arkitektur / Architecture

Projektet är uppdelat i en ren och kommersiell struktur (Real-world coding structure):
1. Product (Model): Datamodell med stark typning för pris (decimal).
2. ProductManager (Business Logic): En isolerad tjänst (Repository-mönster) som sköter datalagring och LINQ-operationer.
3. Program (UI Layer): Hanterar konsolgränssnittet, textfärger och användarinteraktion.

───

🚀 Hur man kör applikationen / How to run

1. Öppna projektet i Visual Studio 2022 eller nyare.
2. Tryck på Ctrl + F5 (Start Without Debugging) för att köra applikationen i konsolen.
3. Alternativt via terminalen:bash
dotnet run

4.
