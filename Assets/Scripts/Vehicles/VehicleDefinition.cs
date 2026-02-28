using System;
using System.Collections.Generic;

namespace LoxQuest3D.Vehicles
{
    [Serializable]
    public sealed class VehicleDefinition
    {
        public string id;
        public string displayName;
        public string description;

        public List<int> allowedLocations = new();
        public int weight = 1;

        // Flavor / future: engine sound tag, color variants
        public string engineSoundTag;
        public List<string> tags = new();
    }
}

