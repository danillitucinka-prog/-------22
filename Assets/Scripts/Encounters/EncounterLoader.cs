using System.IO;
using UnityEngine;

namespace LoxQuest3D.Encounters
{
    public static class EncounterLoader
    {
        // Put encounters.json into Assets/StreamingAssets/encounters.json
        public static EncounterLibrary LoadFromStreamingAssets(string fileName = "encounters.json")
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (!File.Exists(path))
                return new EncounterLibrary();

            var json = File.ReadAllText(path);
            var library = JsonUtility.FromJson<EncounterLibrary>(json);
            return library ?? new EncounterLibrary();
        }
    }
}

