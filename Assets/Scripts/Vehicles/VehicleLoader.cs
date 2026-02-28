using System.IO;
using UnityEngine;

namespace LoxQuest3D.Vehicles
{
    public static class VehicleLoader
    {
        public static VehicleLibrary LoadFromStreamingAssets(string fileName = "vehicles.json")
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (!File.Exists(path))
                return new VehicleLibrary();

            var json = File.ReadAllText(path);
            var lib = JsonUtility.FromJson<VehicleLibrary>(json);
            return lib ?? new VehicleLibrary();
        }
    }
}

