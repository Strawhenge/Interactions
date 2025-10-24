using FunctionalUtilities;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using Strawhenge.Interactions.Unity.Sleep;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class UserContext
    {
        public UserContext(
            EmoteController emoteController,
            SitController sitController,
            SleepController sleepController,
            PositionPlacementController positionPlacementController)
        {
            EmoteController = emoteController;
            SitController = sitController;
            PositionPlacementController = positionPlacementController;
            SleepController = sleepController;
        }

        public EmoteController EmoteController { get; }

        public SitController SitController { get; }

        public SleepController SleepController { get; }

        public PositionPlacementController PositionPlacementController { get; }
    }
}