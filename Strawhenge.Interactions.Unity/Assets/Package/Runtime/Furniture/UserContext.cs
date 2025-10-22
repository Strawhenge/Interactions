using FunctionalUtilities;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class UserContext
    {
        public UserContext(
            EmoteController emoteController,
            SitController sitController,
            PositionPlacementController positionPlacementController)
        {
            EmoteController = emoteController;
            SitController = sitController;
            PositionPlacementController = positionPlacementController;
        }

        public EmoteController EmoteController { get; }

        public SitController SitController { get; }

        public PositionPlacementController PositionPlacementController { get; }
    }
}