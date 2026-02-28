using System;
using System.Collections.Generic;
using System.Linq;
using LoxQuest3D.Core;
using UnityEngine;

namespace LoxQuest3D.NPC
{
    public sealed class NpcRepository
    {
        private readonly List<NpcDefinition> _all;
        private readonly System.Random _rng = new();

        public NpcRepository(IEnumerable<NpcDefinition> npcs)
        {
            _all = npcs?.Where(n => n != null && !string.IsNullOrWhiteSpace(n.id)).ToList() ?? new List<NpcDefinition>();
        }

        public bool TryPick(GameState state, out NpcDefinition npc)
        {
            var candidates = _all.Where(n => IsAllowed(n, state)).ToList();
            if (candidates.Count == 0)
            {
                npc = null;
                return false;
            }

            npc = PickWeighted(candidates);
            return true;
        }

        private NpcDefinition PickWeighted(List<NpcDefinition> candidates)
        {
            var total = 0;
            for (int i = 0; i < candidates.Count; i++)
                total += Mathf.Max(1, candidates[i].weight);

            var roll = _rng.Next(0, Math.Max(1, total));
            for (int i = 0; i < candidates.Count; i++)
            {
                roll -= Mathf.Max(1, candidates[i].weight);
                if (roll < 0)
                    return candidates[i];
            }

            return candidates[candidates.Count - 1];
        }

        private static bool IsAllowed(NpcDefinition n, GameState state)
        {
            if (state.currentDay < n.minDay || state.currentDay > n.maxDay)
                return false;

            if (n.allowedLocations != null && n.allowedLocations.Count > 0)
            {
                if (!n.allowedLocations.Contains(state.locationId))
                    return false;
            }

            return true;
        }
    }
}

