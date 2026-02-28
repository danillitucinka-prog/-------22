using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoxQuest3D.World
{
    [CreateAssetMenu(menuName = "LoxQuest3D/City Theme", fileName = "CityTheme")]
    public sealed class CityTheme : ScriptableObject
    {
        [Header("City identity")]
        public string cityName = "Уныльск";
        public string subtitle = "спальный район, панельки, промышленная тоска";

        [Header("Street name pool")]
        public List<string> streetNames = new()
        {
            "Шахтёрская",
            "Строителей",
            "Космонавтов",
            "Мира",
            "Соборная",
            "Железнодорожная",
            "Центральная",
            "Победы",
            "Юбилейная"
        };

        [Header("Ambient tags (for audio/props later)")]
        public List<string> ambientTags = new()
        {
            "drill_background",
            "distant_train",
            "kiosk_hum",
            "yard_dogs",
            "market_chatter"
        };

        [Header("Visual prop cues (low-poly)")]
        public List<string> propCues = new()
        {
            "panel_house_9floor",
            "rusty_playground",
            "old_bus_stop",
            "dk_building",
            "mine_fence",
            "rail_crossing_gate",
            "garage_row",
            "advert_posters_90s"
        };
    }
}

