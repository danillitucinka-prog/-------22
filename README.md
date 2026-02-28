# Lox Quest 3D (Unity)

MVP scaffold for a 1st-person satirical survival-to-payday game.

## Planned core
- Campaign length selection: 7 / 14 / 30 / 42 days
- Day slots: morning / day / evening / night
- Encounters + dialogue choices (mostly losing money)
- Style tags impacting encounter outcomes
- Save/load (JSON)

## Setup (Unity)
1. Open this folder as a Unity project (recommended: Unity 2022 LTS or newer).
2. On first import, an auto-setup script generates:
   - `Assets/Config/GameConfig.asset`, `Assets/Config/CityTheme.asset`, `Assets/Config/AmbientProfile.asset`
   - a playable scene: `Assets/Scenes/LoxQuest3D_Main.unity`
3. Open `Assets/Scenes/LoxQuest3D_Main.unity` and press Play.

If auto-setup did not run, use the menu: `LoxQuest3D/Setup Project (Generate Scene + Assets)`.

## Notes
- Campaign length is currently set on `GameBootstrapper.campaignLength` (Menu UI can be added next).
- Save file path: `Application.persistentDataPath/loxquest3d.save.json`.

## Content (~160 minutes)
This scaffold uses:
- Hand-authored “set pieces” in `Assets/StreamingAssets/encounters.json`
- Procedural/common encounters in `Assets/Scripts/Encounters/Procedural/EncounterTemplates.cs`
- Interactables (apartment/kiosk/bar etc.) in `Assets/StreamingAssets/interactables.json`

Add more templates + a few longer multi-step quests to reach ~160 minutes without writing hundreds of JSON entries.

## “80s/90s apartment” vibe (implementation)
The vibe is represented as interactables + text (peeling wallpaper, squeaky closet, etc.) in `Assets/StreamingAssets/interactables.json`.
For visuals, create a low-poly apartment scene and place interactable hotspots (UI-driven MVP) or world objects (FPS mode later).

## “Pavlohrad-like” city vibe
This repo keeps the city feel as data + location ids so you can build low-poly scenes consistently:
- Location set: `Assets/Scripts/World/LocationId.cs` (bus station, DК, rail crossing, industrial zone, etc.)
- Optional theme asset: `Assets/Scripts/World/CityTheme.cs` (street names, ambient tags, prop cues)
- Flavor content: `Assets/Scripts/Encounters/Procedural/EncounterTemplates.cs`, `Assets/StreamingAssets/interactables.json`

## Items (satirical)
Basic inventory + consumables are implemented in:
- `Assets/Scripts/Items/Inventory.cs`
- `Assets/Scripts/Gameplay/ConsumableSystem.cs`
- `Assets/Scripts/UI/InventoryPresenter.cs`

## Audio (setup)
This repo includes an ambient audio controller you can wire to imported sounds:
- `Assets/Scripts/Audio/AmbientProfile.cs` (tag -> AudioClips)
- `Assets/Scripts/Audio/AmbientAudioController.cs` (plays loop + random one-shots)

How to use:
1. Import your `.wav`/`.mp3` into Unity (e.g. `Assets/Audio/`).
2. Create an `AmbientProfile` asset and add tags like `distant_train`, `drill_background`, `market_chatter`.
3. Add `AmbientAudioController` to a scene, assign `GameBootstrapper` + `AmbientProfile`.
4. Ensure `GameBootstrapper.cityTheme` is set so ambient tags are available.

## NPCs and cars (data-driven)
Ambient NPCs and vehicles are loaded from:
- `Assets/StreamingAssets/npcs.json`
- `Assets/StreamingAssets/vehicles.json`

For an MVP UI, you can add `Assets/Scripts/UI/AmbientSpawnPresenter.cs` to show what NPC/car is “around” at the current location.

## Local network (LAN co-op, optional)
LAN scripts are behind `LOXQUEST_NETCODE` compile define to avoid requiring packages by default.
To enable:
1. Install packages in Unity: Netcode for GameObjects + Unity Transport.
2. Add Scripting Define Symbol: `LOXQUEST_NETCODE`
3. Add to a scene: `NetworkManager` + `UnityTransport`, `LanLobbyUI`, and `LanCoopManager` (for state sync).
