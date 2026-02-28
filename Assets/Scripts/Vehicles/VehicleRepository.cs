using System;
using System.Collections.Generic;
using System.Linq;
using LoxQuest3D.Core;
using UnityEngine;

namespace LoxQuest3D.Vehicles
{
    public sealed class VehicleRepository
    {
        private readonly List<VehicleDefinition> _all;
        private readonly System.Random _rng = new();

        public VehicleRepository(IEnumerable<VehicleDefinition> vehicles)
        {
            _all = vehicles?.Where(v => v != null && !string.IsNullOrWhiteSpace(v.id)).ToList() ?? new List<VehicleDefinition>();
        }

        public bool TryPick(GameState state, out VehicleDefinition vehicle)
        {
            var candidates = _all.Where(v => IsAllowed(v, state)).ToList();
            if (candidates.Count == 0)
            {
                vehicle = null;
                return false;
            }

            vehicle = PickWeighted(candidates);
            return true;
        }

        private VehicleDefinition PickWeighted(List<VehicleDefinition> candidates)
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

        private static bool IsAllowed(VehicleDefinition v, GameState state)
        {
            if (v.allowedLocations != null && v.allowedLocations.Count > 0)
            {
                if (!v.allowedLocations.Contains(state.locationId))
                    return false;
            }
            return true;
        }
    }
}

