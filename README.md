# Lox Quest 3D (Unity)

MVP scaffold for a 1st-person satirical survival-to-payday game.

## Planned core
- Campaign length selection: 7 / 14 / 30 / 42 days
- Day slots: morning / day / evening / night
- Encounters + dialogue choices (mostly losing money)
- Style tags impacting encounter outcomes
- Save/load (JSON)

## Setup (Unity)
1. Create a new Unity project with this folder as the project root (recommended: Unity 2022 LTS or newer).
2. In Unity, create a `GameConfig` asset: `Assets/Create/LoxQuest3D/Game Config`.
3. Create an empty scene and add:
   - an empty GameObject with `GameBootstrapper` (assign the `GameConfig` asset)
   - a Canvas with:
     - `Text` fields for title/body/result
     - 2–4 `Button`s for choices
     - `EncounterPresenter` (assign refs + the `GameBootstrapper`)
4. Ensure `Assets/StreamingAssets/encounters.json` exists (already included) to feed encounters.

## Notes
- Campaign length is currently set on `GameBootstrapper.campaignLength` (Menu UI can be added next).
- Save file path: `Application.persistentDataPath/loxquest3d.save.json`.
