using System;
using System.Collections.Generic;

namespace LoxQuest3D.Vehicles
{
    [Serializable]
    public sealed class VehicleLibrary
    {
        public List<VehicleDefinition> vehicles = new();
    }
}

