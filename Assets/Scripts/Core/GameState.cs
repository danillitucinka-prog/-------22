using System;
using System.Collections.Generic;
using LoxQuest3D.Items;

namespace LoxQuest3D.Core
{
    [Serializable]
    public sealed class GameState
    {
        public int targetDayCount;
        public int currentDay; // 1-based
        public DaySlot currentSlot;

        // Stored as int for JSON compatibility / forward-compat with enums.
        public int locationId;

        public int money;
        public int stress;

        // Joke-axiom: always minimal; kept for UI/logic hooks.
        public int luck = int.MinValue;

        public List<string> styleTags = new();
        public Inventory inventory = new();

        public static GameState New(int targetDayCount, int startingMoney, int startingStress)
        {
            return new GameState
            {
                targetDayCount = targetDayCount,
                currentDay = 1,
                currentSlot = DaySlot.Morning,
                locationId = 0,
                money = startingMoney,
                stress = startingStress,
                luck = int.MinValue,
                styleTags = new List<string>(),
                inventory = new Inventory()
            };
        }
    }
}
