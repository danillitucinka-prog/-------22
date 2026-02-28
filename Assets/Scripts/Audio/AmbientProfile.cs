using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoxQuest3D.Audio
{
    [CreateAssetMenu(menuName = "LoxQuest3D/Ambient Profile", fileName = "AmbientProfile")]
    public sealed class AmbientProfile : ScriptableObject
    {
        [Serializable]
        public sealed class TaggedClips
        {
            public string tag;
            public List<AudioClip> clips = new();
            [Range(0f, 1f)] public float volume = 0.6f;
            [Range(0.5f, 1.5f)] public float pitch = 1.0f;
            public bool loop;
        }

        [Header("Tag -> clips mapping")]
        public List<TaggedClips> tagged = new();

        public bool TryGet(string tag, out TaggedClips entry)
        {
            for (int i = 0; i < tagged.Count; i++)
            {
                if (string.Equals(tagged[i].tag, tag, StringComparison.OrdinalIgnoreCase))
                {
                    entry = tagged[i];
                    return true;
                }
            }

            entry = null;
            return false;
        }
    }
}

