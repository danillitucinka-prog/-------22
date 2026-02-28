using LoxQuest3D.Core;
using LoxQuest3D.Encounters;

namespace LoxQuest3D.Gameplay
{
    public static class ChoiceApplier
    {
        public static void Apply(GameState state, EncounterChoice choice, int maxStress)
        {
            state.money += choice.moneyDelta;
            state.stress = Clamp(state.stress + choice.stressDelta, 0, maxStress);

            if (choice.addStyleTags != null)
            {
                for (int i = 0; i < choice.addStyleTags.Count; i++)
                {
                    var tag = choice.addStyleTags[i];
                    if (!string.IsNullOrWhiteSpace(tag) && !state.styleTags.Contains(tag))
                        state.styleTags.Add(tag);
                }
            }

            if (choice.removeStyleTags != null)
            {
                for (int i = 0; i < choice.removeStyleTags.Count; i++)
                {
                    state.styleTags.Remove(choice.removeStyleTags[i]);
                }
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

