using System;
using System.Collections.Generic;

namespace LoxQuest3D.Encounters
{
    [Serializable]
    public sealed class EncounterLibrary
    {
        public List<EncounterDefinition> encounters = new();
    }
}

