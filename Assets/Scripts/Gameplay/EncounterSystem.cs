using LoxQuest3D.Core;
using LoxQuest3D.Encounters;
using UnityEngine;

namespace LoxQuest3D.Gameplay
{
    public sealed class EncounterSystem
    {
        private readonly EncounterRepository _repo;
        private readonly GameConfig _config;

        public EncounterSystem(EncounterRepository repo, GameConfig config)
        {
            _repo = repo;
            _config = config;
        }

        public int RollEncounterCountForSlot()
        {
            var min = Mathf.Min(_config.encountersPerSlotMin, _config.encountersPerSlotMax);
            var max = Mathf.Max(_config.encountersPerSlotMin, _config.encountersPerSlotMax);
            return Random.Range(min, max + 1);
        }

        public bool TryGetEncounter(GameState state, out EncounterDefinition encounter)
            => _repo.TryPick(state, out encounter);
    }
}

