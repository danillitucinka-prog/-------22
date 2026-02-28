using System;
using System.IO;
using LoxQuest3D.Core;
using UnityEngine;

namespace LoxQuest3D.IO
{
    public static class SaveSystem
    {
        private const string FileName = "loxquest3d.save.json";

        public static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

        public static void Save(GameState state)
        {
            var json = JsonUtility.ToJson(state, prettyPrint: true);
            File.WriteAllText(SavePath, json);
        }

        public static bool TryLoad(out GameState state)
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    state = null;
                    return false;
                }

                var json = File.ReadAllText(SavePath);
                state = JsonUtility.FromJson<GameState>(json);
                return state != null;
            }
            catch
            {
                state = null;
                return false;
            }
        }

        public static void Delete()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}

