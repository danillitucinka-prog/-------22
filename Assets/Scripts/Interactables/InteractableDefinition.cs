using System;
using System.Collections.Generic;

namespace LoxQuest3D.Interactables
{
    [Serializable]
    public sealed class InteractableDefinition
    {
        public string id;
        public string title;
        public string body;

        // Gating
        public List<int> allowedLocations = new();
        public int minDay = 1;
        public int maxDay = 999;

        public List<InteractableAction> actions = new();
        public int weight = 1;
    }

    [Serializable]
    public sealed class InteractableAction
    {
        public string label;
        public int moneyDelta;
        public int stressDelta;

        // Inventory ops
        public int giveItemId;
        public int giveItemCount;
        public int takeItemId;
        public int takeItemCount;

        // Style ops
        public List<string> addStyleTags = new();
        public List<string> removeStyleTags = new();

        public string resultText;
    }
}

