# MyRevitTools – Revit add-in

Revit 2023, 2024, 2025, 2026 ke liye C# add-in. Ek codebase, kai versions.

## Build

```
dotnet build src/MyRevitTools/MyRevitTools.csproj -c Debug -p:RevitVersion=2024
```

- `RevitVersion` 2023/2024 -> `net48`; 2025+ -> `net8.0-windows`.
- Debug build Windows par add-in ko `%AppData%\Autodesk\Revit\Addins\<version>\` me copy kar deta hai.
- Saare versions: `build/build-all.ps1`.
- Build se pehle Revit band karo, warna DLL lock rahegi.

## Revit API rules (zaroor follow karo)

- Model badalne wala har kaam `Transaction` ke andar ho. Sirf padhne wale commands par `[Transaction(TransactionMode.ReadOnly)]`.
- `ElementId` ka number chahiye to `Utils/ElementIdExtensions.GetIdValue()` use karo, seedha `IntegerValue` / `Value` nahi (2024 me API badla tha).
- Version-specific code ke liye `#if REVIT2024_OR_GREATER`, `REVIT2025_OR_GREATER`, `REVIT2026_OR_GREATER`.
- Modeless WPF window se Revit API seedha call mat karo; `ExternalEvent` + `IExternalEventHandler` use karo.
- `PickObject` ka Esc `OperationCanceledException` deta hai: pakdo aur `Result.Cancelled` return karo.
- `.NET 8` only APIs (jaise naye LINQ methods) net48 par nahi chalenge; dono targets par compile hona chahiye.
- Units ke liye `UnitTypeId` / `ForgeTypeId` use karo, purane `DisplayUnitType` nahi.

## Structure

- `App.cs` – `IExternalApplication`, Ribbon tab "My Tools" banata hai
- `Commands/` – har button ka ek `IExternalCommand`
- `Utils/` – version-compatibility helpers
- `MyRevitTools.addin` – Revit manifest

Naya command add karna: `Commands/` me class banao, phir `App.OnStartup` me `AddButton<...>` call karo.
