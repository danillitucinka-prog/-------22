using System.Collections.Generic;
using LoxQuest3D.World;
using UnityEngine;

namespace LoxQuest3D.Audio
{
    public sealed class AmbientAudioController : MonoBehaviour
    {
        [Header("Refs")]
        public LoxQuest3D.Scenes.GameBootstrapper bootstrapper;
        public AmbientProfile profile;

        [Header("Sources")]
        public AudioSource loopSource;
        public AudioSource oneshotSource;

        [Header("Behavior")]
        [Tooltip("Seconds between random one-shots from theme tags.")]
        public Vector2 oneshotIntervalSeconds = new(18f, 35f);

        private readonly List<string> _tags = new();
        private float _nextOneshotTime;

        private void Awake()
        {
            if (loopSource == null)
            {
                loopSource = gameObject.AddComponent<AudioSource>();
                loopSource.loop = true;
                loopSource.playOnAwake = false;
            }

            if (oneshotSource == null)
            {
                oneshotSource = gameObject.AddComponent<AudioSource>();
                oneshotSource.loop = false;
                oneshotSource.playOnAwake = false;
            }
        }

        private void Start()
        {
            RefreshTags();
            ScheduleNextOneshot();
            PlayLoopForCurrentLocation();
        }

        private void Update()
        {
            if (bootstrapper == null || bootstrapper.Context == null) return;

            if (Time.unscaledTime >= _nextOneshotTime)
            {
                PlayRandomOneshot();
                ScheduleNextOneshot();
            }
        }

        public void RefreshTags()
        {
            _tags.Clear();
            if (bootstrapper == null || bootstrapper.cityTheme == null) return;
            if (bootstrapper.cityTheme.ambientTags == null) return;

            for (int i = 0; i < bootstrapper.cityTheme.ambientTags.Count; i++)
            {
                var t = bootstrapper.cityTheme.ambientTags[i];
                if (!string.IsNullOrWhiteSpace(t))
                    _tags.Add(t.Trim());
            }
        }

        public void PlayLoopForCurrentLocation()
        {
            if (profile == null || bootstrapper == null) return;

            var loc = (LocationId)bootstrapper.Context.State.locationId;
            var tag = LocationDefaultLoopTag(loc);
            if (string.IsNullOrWhiteSpace(tag)) return;

            if (!profile.TryGet(tag, out var entry) || entry.clips == null || entry.clips.Count == 0)
                return;

            var clip = entry.clips[Random.Range(0, entry.clips.Count)];
            loopSource.clip = clip;
            loopSource.volume = entry.volume;
            loopSource.pitch = entry.pitch;
            loopSource.loop = true;
            loopSource.Play();
        }

        private void PlayRandomOneshot()
        {
            if (profile == null || _tags.Count == 0) return;

            var tag = _tags[Random.Range(0, _tags.Count)];
            if (!profile.TryGet(tag, out var entry) || entry.clips == null || entry.clips.Count == 0)
                return;

            var clip = entry.clips[Random.Range(0, entry.clips.Count)];
            oneshotSource.pitch = entry.pitch;
            oneshotSource.PlayOneShot(clip, entry.volume);
        }

        private void ScheduleNextOneshot()
        {
            var min = Mathf.Min(oneshotIntervalSeconds.x, oneshotIntervalSeconds.y);
            var max = Mathf.Max(oneshotIntervalSeconds.x, oneshotIntervalSeconds.y);
            _nextOneshotTime = Time.unscaledTime + Random.Range(min, max);
        }

        private static string LocationDefaultLoopTag(LocationId loc)
        {
            return loc switch
            {
                LocationId.Apartment => "apartment_hum",
                LocationId.Entrance => "entrance_echo",
                LocationId.Yard or LocationId.PanelDistrict => "yard_wind",
                LocationId.Market or LocationId.MarketSquare => "market_chatter",
                LocationId.BusStop or LocationId.BusStation => "traffic_distant",
                LocationId.RailCrossing => "distant_train",
                LocationId.IndustrialZone or LocationId.MineGate => "industrial_hum",
                LocationId.Cafe or LocationId.Bar => "bar_murmur",
                _ => null
            };
        }
    }
}

