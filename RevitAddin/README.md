# MyRevitTools

Revit 2023+ ke liye starter add-in. Ribbon par "My Tools" tab, jisme 2 buttons hain:

- **Element Info** – element pick karo, uski details aur saare parameters dekho
- **Count Elements** – active view me har category ke kitne elements hain

## Zaroori cheezein (Windows)

1. Revit 2023 / 2024 / 2025 / 2026 (koi bhi ek)
2. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
3. Visual Studio 2022 ya JetBrains Rider (optional, debugging ke liye)

## Pehli baar chalana

```powershell
cd RevitAddin
dotnet build src\MyRevitTools\MyRevitTools.csproj -c Debug -p:RevitVersion=2024
```

`2024` ki jagah apna Revit version likho. Build ke baad Revit kholo: ribbon par **My Tools** tab dikhega.
Pehli baar Revit "Load Always / Load Once" poochega, **Load Always** chuno.

## Debugging (Visual Studio)

Project properties > Debug > Start external program:
`C:\Program Files\Autodesk\Revit 2024\Revit.exe`. Phir F5 dabao, breakpoints kaam karenge.
