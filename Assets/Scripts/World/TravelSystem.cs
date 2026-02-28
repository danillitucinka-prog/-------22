using LoxQuest3D.Core;

namespace LoxQuest3D.World
{
    public static class TravelSystem
    {
        public static void TravelTo(GameState state, LocationId location)
        {
            state.locationId = (int)location;
        }
    }
}
