using LoxQuest3D.Core;

namespace LoxQuest3D.Gameplay
{
    public static class DayCycle
    {
        public static bool AdvanceSlot(GameState state)
        {
            if (state.currentSlot == DaySlot.Night)
            {
                state.currentSlot = DaySlot.Morning;
                state.currentDay += 1;
                return true; // new day
            }

            state.currentSlot = (DaySlot)((int)state.currentSlot + 1);
            return false;
        }
    }
}

