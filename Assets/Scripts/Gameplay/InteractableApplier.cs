using LoxQuest3D.Core;
using LoxQuest3D.Interactables;
using LoxQuest3D.Items;

namespace LoxQuest3D.Gameplay
{
    public static class InteractableApplier
    {
        public static bool CanApply(GameState state, InteractableAction action)
        {
            if (action.takeItemId != 0 && action.takeItemCount > 0)
            {
                var id = (ItemId)action.takeItemId;
                return state.inventory.GetCount(id) >= action.takeItemCount;
            }
            return true;
        }

        public static void Apply(GameState state, InteractableAction action, int maxStress)
        {
            state.money += action.moneyDelta;
            state.stress = Clamp(state.stress + action.stressDelta, 0, maxStress);

            if (action.takeItemId != 0 && action.takeItemCount > 0)
                state.inventory.TryRemove((ItemId)action.takeItemId, action.takeItemCount);

            if (action.giveItemId != 0 && action.giveItemCount > 0)
                state.inventory.Add((ItemId)action.giveItemId, action.giveItemCount);

            if (action.addStyleTags != null)
            {
                for (int i = 0; i < action.addStyleTags.Count; i++)
                {
                    var tag = action.addStyleTags[i];
                    if (!string.IsNullOrWhiteSpace(tag) && !state.styleTags.Contains(tag))
                        state.styleTags.Add(tag);
                }
            }

            if (action.removeStyleTags != null)
            {
                for (int i = 0; i < action.removeStyleTags.Count; i++)
                    state.styleTags.Remove(action.removeStyleTags[i]);
            }
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}

