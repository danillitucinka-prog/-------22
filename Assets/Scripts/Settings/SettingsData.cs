using System;

namespace LoxQuest3D.Settings
{
    [Serializable]
    public sealed class SettingsData
    {
        public float masterVolume = 0.8f;
        public float mouseSensitivity = 1.0f;
        public int targetFps = 60;
    }
}

