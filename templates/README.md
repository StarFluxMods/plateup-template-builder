# My Mod

A PlateUp! mod by My Name.

<!-- A short description of what your mod does. -->

## Features

- Describe the first thing your mod adds or changes.
- Describe the next thing your mod adds or changes.

## Requirements

- [PlateUp!](https://store.steampowered.com/app/1599600/PlateUp/)
<!-- kitchenlib -->
- [KitchenLib](https://github.com/KitchenMods/KitchenLib)
<!-- /kitchenlib -->

## Installation

Subscribe to the mod on the Steam Workshop.

## Development

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) and an IDE such as Visual Studio or Rider
- PlateUp! installed through Steam
<!-- unity -->
- [Unity Hub](https://unity.com/download) with Unity **2020.3.49f1**
<!-- /unity -->

### Building

<!-- yariazen -->
Open `MyMod.csproj` in your IDE and build. The [Yariazen.PlateUp.ModBuildUtilities](https://github.com/Yariazen/Yaraizen.PlateUp.ModBuildUtilities) package pulls in the game's references and copies the built mod to your PlateUp! mods folder automatically.
<!-- /yariazen -->
<!-- starflux -->
Open `MyMod.csproj` in your IDE and build. The [StarFluxGames.PlateUp.ModBuildUtilities](https://www.nuget.org/packages/StarFluxGames.PlateUp.ModBuildUtilities) package pulls in the game's references and copies the built mod to your PlateUp! mods folder automatically.
<!-- /starflux -->
<!-- unity -->

### Building assets

1. Open `UnityProject - MyMod` in Unity Hub.
2. Tag any assets you want to ship with the `mod.assets` AssetBundle (via the AssetBundle dropdown at the bottom of the Inspector).
3. Select **PlateUp! > Build Asset Bundle** (or press F6). The bundle is written to `UnityProject - MyMod/content/mod.assets`.
4. Rebuild the mod project so the bundle is copied alongside your mod.
<!-- /unity -->

## Changelog

Current version: **0.1.0**
<!-- changelogs -->

See the [Changelogs](Changelogs/GitHub) folder for release notes.
<!-- /changelogs -->

## Credits

- My Name
<!-- customs -->
- Example assets from Vanilla PlateUp! and [DepletedNova](https://github.com/DepletedNova)
<!-- /customs -->
