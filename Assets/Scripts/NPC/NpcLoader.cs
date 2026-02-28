using System.IO;
using UnityEngine;

namespace LoxQuest3D.NPC
{
    public static class NpcLoader
    {
        public static NpcLibrary LoadFromStreamingAssets(string fileName = "npcs.json")
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (!File.Exists(path))
                return new NpcLibrary();

            var json = File.ReadAllText(path);
            var lib = JsonUtility.FromJson<NpcLibrary>(json);
            return lib ?? new NpcLibrary();
        }
    }
}

