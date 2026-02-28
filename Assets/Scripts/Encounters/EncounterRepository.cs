using System;
using System.Collections.Generic;
using System.Linq;
using LoxQuest3D.Core;
using UnityEngine;

namespace LoxQuest3D.Encounters
{
    public sealed class EncounterRepository
    {
        private readonly List<EncounterDefinition> _all;
        private readonly System.Random _rng = new();

        public EncounterRepository(IEnumerable<EncounterDefinition> encounters)
        {
            _all = encounters?.Where(e => e != null && !string.IsNullOrWhiteSpace(e.id)).ToList() ?? new List<EncounterDefinition>();
        }

        public bool TryPick(GameState state, out EncounterDefinition encounter)
        {
            var candidates = _all.Where(e => IsAllowed(e, state)).ToList();
            if (candidates.Count == 0)
            {
                encounter = null;
                return false;
            }

            encounter = PickWeighted(candidates);
            return true;
        }

        private EncounterDefinition PickWeighted(List<EncounterDefinition> candidates)
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

        private static bool IsAllowed(EncounterDefinition e, GameState state)
        {
            if (state.currentDay < e.minDay || state.currentDay > e.maxDay)
                return false;

            if (e.allowedLocations != null && e.allowedLocations.Count > 0)
            {
                if (!e.allowedLocations.Contains(state.locationId))
                    return false;
            }

            if (e.requiredStyleTags != null && e.requiredStyleTags.Count > 0)
            {
                for (int i = 0; i < e.requiredStyleTags.Count; i++)
                {
                    if (!state.styleTags.Contains(e.requiredStyleTags[i]))
                        return false;
                }
            }

            if (e.forbiddenStyleTags != null && e.forbiddenStyleTags.Count > 0)
            {
                for (int i = 0; i < e.forbiddenStyleTags.Count; i++)
                {
                    if (state.styleTags.Contains(e.forbiddenStyleTags[i]))
                        return false;
                }
            }

            return true;
        }
    }
}
