using LoxQuest3D.Core;
using LoxQuest3D.Items;

namespace LoxQuest3D.Gameplay
{
    public static class ConsumableSystem
    {
        public static bool TryUse(GameState state, ItemId item, int maxStress, out string result)
        {
            if (state.inventory.GetCount(item) <= 0)
            {
                result = "Нечего использовать.";
                return false;
            }

            switch (item)
            {
                case ItemId.Cigarettes:
                    state.inventory.TryRemove(item, 1);
                    state.stress = Clamp(state.stress - 2, 0, maxStress);
                    result = "Ты закурил. На минуту стало тише внутри.";
                    return true;

                case ItemId.CheapBeer:
                    state.inventory.TryRemove(item, 1);
                    state.stress = Clamp(state.stress - 3, 0, maxStress);
                    state.money -= 30; // “закуска”
                    result = "Пиво ушло. Закуска тоже внезапно стала обязательной.";
                    return true;

                case ItemId.VodkaSmall:
                    state.inventory.TryRemove(item, 1);
                    state.stress = Clamp(state.stress - 4, 0, maxStress);
                    state.money -= 80; // “такси домой”
                    result = "Ты выпил. Потом заплатил за «такси домой», хотя шёл пешком.";
                    return true;

                case ItemId.InstantNoodles:
                    state.inventory.TryRemove(item, 1);
                    state.stress = Clamp(state.stress - 1, 0, maxStress);
                    result = "Лапша спасла вечер. Желудок не в восторге, но ты ещё тут.";
                    return true;
            }

            result = "Это нельзя использовать.";
            return false;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}

