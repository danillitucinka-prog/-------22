using System;
using System.Collections.Generic;
using System.Linq;
using LoxQuest3D.Core;
using UnityEngine;

namespace LoxQuest3D.Interactables
{
    public sealed class InteractableRepository
    {
        private readonly List<InteractableDefinition> _all;
        private readonly System.Random _rng = new();

        public InteractableRepository(IEnumerable<InteractableDefinition> interactables)
        {
            _all = interactables?.Where(i => i != null && !string.IsNullOrWhiteSpace(i.id)).ToList() ?? new List<InteractableDefinition>();
        }

        public bool TryPick(GameState state, out InteractableDefinition interactable)
        {
            var candidates = _all.Where(i => IsAllowed(i, state)).ToList();
            if (candidates.Count == 0)
            {
                interactable = null;
                return false;
            }

            interactable = PickWeighted(candidates);
            return true;
        }

        private InteractableDefinition PickWeighted(List<InteractableDefinition> candidates)
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

        private static bool IsAllowed(InteractableDefinition i, GameState state)
        {
            if (state.currentDay < i.minDay || state.currentDay > i.maxDay)
                return false;

            if (i.allowedLocations != null && i.allowedLocations.Count > 0)
            {
                if (!i.allowedLocations.Contains(state.locationId))
                    return false;
            }

            return true;
        }
    }
}

