using UnityEngine;

namespace LoxQuest3D.Core
{
    [CreateAssetMenu(menuName = "LoxQuest3D/Game Config", fileName = "GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Economy")]
        public int startingMoney = 3000;

        [Header("Stress")]
        public int startingStress = 0;
        public int maxStress = 100;

        [Header("Encounter pacing")]
        [Range(0, 2)] public int encountersPerSlotMin = 1;
        [Range(0, 3)] public int encountersPerSlotMax = 2;
    }
}

