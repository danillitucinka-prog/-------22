using UnityEngine;

namespace LoxQuest3D.Settings
{
    public static class SettingsSystem
    {
        private const string Key = "loxquest3d.settings.json";
        private static SettingsData _cached;

        public static SettingsData Load()
        {
            if (_cached != null) return _cached;
            if (!PlayerPrefs.HasKey(Key))
            {
                _cached = new SettingsData();
                Apply(_cached);
                return _cached;
            }

            var json = PlayerPrefs.GetString(Key, "");
            try
            {
                _cached = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();
            }
            catch
            {
                _cached = new SettingsData();
            }

            Apply(_cached);
            return _cached;
        }

        public static void Save(SettingsData data)
        {
            _cached = data;
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
            Apply(data);
        }

        public static void Apply(SettingsData data)
        {
            AudioListener.volume = Mathf.Clamp01(data.masterVolume);
            Application.targetFrameRate = data.targetFps <= 0 ? -1 : data.targetFps;
        }
    }
}

