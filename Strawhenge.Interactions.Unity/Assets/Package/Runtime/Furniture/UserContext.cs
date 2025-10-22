using FunctionalUtilities;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class UserContext
    {
        public UserContext(
            EmoteController emoteController,
            PositionPlacementController positionPlacementController)
        {
            EmoteController = emoteController;
            PositionPlacementController = positionPlacementController;
        }

        public EmoteController EmoteController { get; }

        public PositionPlacementController PositionPlacementController { get; }
    }
}