# DiscordBot

Projekt prostego, modułowego **bota Discord** napisanego w **C#** z wykorzystaniem biblioteki **Discord.Net**.  
Bot został zaprojektowany w sposób umożliwiający łatwą rozbudowę o nowe komendy, moduły oraz systemy zarządzania danymi

---

## 🚀 Funkcje

- 🔌 **Pełna integracja** z Discord API
Połączenie z Discordem przy użyciu biblioteki Discord.Net, obsługa zdarzeń oraz komunikacji w czasie rzeczywistym.
- 🧩 **Modularna architektura projektu**
Logika bota została podzielona na wyraźne warstwy (Manager, DataManager, Extensions), co poprawia czytelność i ułatwia rozwój.
- 📝 **System logów i zdarzeń**
Rejestrowanie kluczowych zdarzeń, takich jak start bota, błędy, komunikaty systemowe oraz działania użytkowników.
- 👥 **Zarządzanie użytkownikami serwera**
Pobieranie danych użytkowników Discorda oraz ich przetwarzanie w modelach aplikacji.
- 📦 **Czytelna struktura katalogów**
Projekt podzielony logicznie na katalogi odpowiedzialne za konkretne zadania.
- ⚙️ **Solidna baza pod dalszy rozwój**
Projekt jest przygotowany do rozbudowy o:
    - komendy tekstowe i slash commands,
    - system ról i uprawnień,
    - bazę danych,
    - integracje z zewnętrznymi API.

---

## 🛠️ Technologie

- **Język:** C#
- **Framework:** .NET (net9.0)
- **Biblioteka:** Discord.Net
- **Typ aplikacji:** Console App
- **Styl architektury**: modularny / warstwowy

---

## 📁 Struktura projektu

```
MyBot/
├── Exceptions/
├── DataManager/
├── Enums/
├── Extensions/
├── Messages/
├── Models/
├── MyBotManager.cs
└── Program.cs
```

### 📂 Exceptions

Zawiera klasy odpowiedzialne za obsługę wyjątków aplikacji.

### 📂 DataManager

Warstwa odpowiedzialna za zarządzanie danymi oraz logami aplikacji.

### 📂 Enums

Zawiera enumeracje wykorzystywane w całym projekcie.

### 📂 Extensions

Metody rozszerzające istniejące klasy.

### 📂 Messages

Odpowiada za obsługę wiadomości i komend bota.

### 📂 Models

Zawiera modele danych wykorzystywane przez aplikację.

### 📄 MyBotManager.cs

Główna klasa zarządzająca cyklem życia bota.

### 📄 Program.cs

Punkt wejścia aplikacji.