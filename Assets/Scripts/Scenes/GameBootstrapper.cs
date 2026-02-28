using LoxQuest3D.Core;
using LoxQuest3D.Encounters;
using LoxQuest3D.Encounters.Procedural;
using LoxQuest3D.Gameplay;
using LoxQuest3D.Interactables;
using LoxQuest3D.IO;
using LoxQuest3D.NPC;
using LoxQuest3D.Settings;
using LoxQuest3D.Vehicles;
using UnityEngine;

namespace LoxQuest3D.Scenes
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        [Header("Config")]
        public GameConfig config;
        public LoxQuest3D.World.CityTheme cityTheme;

        [Header("Runtime (debug)")]
        public bool autoStartNewGame = true;
        public CampaignLength campaignLength = CampaignLength.Days7;

        public RunContext Context { get; private set; }
        public EncounterSystem EncounterSystem { get; private set; }
        public InteractableSystem InteractableSystem { get; private set; }
        public NpcRepository Npcs { get; private set; }
        public VehicleRepository Vehicles { get; private set; }

        private void Awake()
        {
            if (config == null)
            {
                Debug.LogError("GameBootstrapper: missing GameConfig");
                enabled = false;
                return;
            }

            if (cityTheme == null)
                Debug.LogWarning("GameBootstrapper: CityTheme not set (optional)");

            if (SaveSystem.TryLoad(out var loaded))
                Context = new RunContext(loaded);
            else if (autoStartNewGame)
            {
                var len = (int)campaignLength;
                // If started from Main Menu, use selection.
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == SceneIds.City)
                    len = (int)RunConfig.CampaignLength;
                Context = new RunContext(GameState.New(len, config.startingMoney, config.startingStress));
            }
            else
                Context = new RunContext(GameState.New((int)CampaignLength.Days7, config.startingMoney, config.startingStress));

            var library = EncounterLoader.LoadFromStreamingAssets();
            var allEncounters = new System.Collections.Generic.List<EncounterDefinition>(library.encounters ?? new System.Collections.Generic.List<EncounterDefinition>());
            allEncounters.AddRange(EncounterTemplates.BuildCommon());
            var repo = new EncounterRepository(allEncounters);
            EncounterSystem = new EncounterSystem(repo, config);

            var interactablesLib = InteractableLoader.LoadFromStreamingAssets();
            var interactablesRepo = new InteractableRepository(interactablesLib.interactables);
            InteractableSystem = new InteractableSystem(interactablesRepo);

            var npcLib = NpcLoader.LoadFromStreamingAssets();
            Npcs = new NpcRepository(npcLib.npcs);

            var vehicleLib = VehicleLoader.LoadFromStreamingAssets();
            Vehicles = new VehicleRepository(vehicleLib.vehicles);
        }

        private void OnApplicationQuit()
        {
            if (Context?.State != null)
                SaveSystem.Save(Context.State);
        }
    }
}
