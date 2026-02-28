using System;
using System.Collections.Generic;

namespace LoxQuest3D.Encounters
{
    [Serializable]
    public sealed class EncounterDefinition
    {
        public string id;
        public string title;
        public string body;

        public List<EncounterChoice> choices = new();

        // Optional gating
        public List<int> allowedLocations = new();
        public List<string> requiredStyleTags = new();
        public List<string> forbiddenStyleTags = new();
        public int minDay = 1;
        public int maxDay = 999;

        // Weight for random picking (>=1).
        public int weight = 1;
    }

    [Serializable]
    public sealed class EncounterChoice
    {
        public string label;
        public int moneyDelta;
        public int stressDelta;
        public List<string> addStyleTags = new();
        public List<string> removeStyleTags = new();
        public string resultText;
    }
}
