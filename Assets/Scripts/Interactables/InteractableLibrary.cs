using System;
using System.Collections.Generic;

namespace LoxQuest3D.Interactables
{
    [Serializable]
    public sealed class InteractableLibrary
    {
        public List<InteractableDefinition> interactables = new();
    }
}

