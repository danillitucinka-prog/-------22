using System;
using System.Collections.Generic;

namespace LoxQuest3D.NPC
{
    [Serializable]
    public sealed class NpcLibrary
    {
        public List<NpcDefinition> npcs = new();
    }
}

