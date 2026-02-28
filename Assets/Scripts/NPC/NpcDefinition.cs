using System;
using System.Collections.Generic;

namespace LoxQuest3D.NPC
{
    [Serializable]
    public sealed class NpcDefinition
    {
        public string id;
        public string displayName;
        public string description;

        public List<int> allowedLocations = new();
        public int minDay = 1;
        public int maxDay = 999;
        public int weight = 1;

        // Optional: NPC can start an encounter id (from encounter pool) or an interactable id.
        public string linkedEncounterId;
        public string linkedInteractableId;

        // Flavor tags for later (voice, outfit, faction)
        public List<string> tags = new();
    }
}

