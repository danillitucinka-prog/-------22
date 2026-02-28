using LoxQuest3D.Core;

namespace LoxQuest3D.Settings
{
    public static class RunConfig
    {
        public static CampaignLength CampaignLength { get; private set; } = CampaignLength.Days14;
        public static bool NewGame { get; private set; } = true;

        public static void Set(CampaignLength length, bool newGame)
        {
            CampaignLength = length;
            NewGame = newGame;
        }
    }
}

