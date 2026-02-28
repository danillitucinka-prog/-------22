using System.IO;
using UnityEngine;

namespace LoxQuest3D.Interactables
{
    public static class InteractableLoader
    {
        public static InteractableLibrary LoadFromStreamingAssets(string fileName = "interactables.json")
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (!File.Exists(path))
                return new InteractableLibrary();

            var json = File.ReadAllText(path);
            var lib = JsonUtility.FromJson<InteractableLibrary>(json);
            return lib ?? new InteractableLibrary();
        }
    }
}

