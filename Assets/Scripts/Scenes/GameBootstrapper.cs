using LoxQuest3D.Core;
using LoxQuest3D.Encounters;
using LoxQuest3D.Encounters.Procedural;
using LoxQuest3D.Gameplay;
using LoxQuest3D.IO;
using UnityEngine;

namespace LoxQuest3D.Scenes
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        [Header("Config")]
        public GameConfig config;

        [Header("Runtime (debug)")]
        public bool autoStartNewGame = true;
        public CampaignLength campaignLength = CampaignLength.Days7;

        public RunContext Context { get; private set; }
        public EncounterSystem EncounterSystem { get; private set; }

        private void Awake()
        {
            if (config == null)
            {
                Debug.LogError("GameBootstrapper: missing GameConfig");
                enabled = false;
                return;
            }

            if (SaveSystem.TryLoad(out var loaded))
                Context = new RunContext(loaded);
            else if (autoStartNewGame)
                Context = new RunContext(GameState.New((int)campaignLength, config.startingMoney, config.startingStress));
            else
                Context = new RunContext(GameState.New((int)CampaignLength.Days7, config.startingMoney, config.startingStress));

            var library = EncounterLoader.LoadFromStreamingAssets();
            var allEncounters = new System.Collections.Generic.List<EncounterDefinition>(library.encounters ?? new System.Collections.Generic.List<EncounterDefinition>());
            allEncounters.AddRange(EncounterTemplates.BuildCommon());
            var repo = new EncounterRepository(allEncounters);
            EncounterSystem = new EncounterSystem(repo, config);
        }

        private void OnApplicationQuit()
        {
            if (Context?.State != null)
                SaveSystem.Save(Context.State);
        }
    }
}
