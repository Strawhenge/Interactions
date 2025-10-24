using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sleep;

namespace Strawhenge.Interactions.Unity
{
    public class Bed : Furniture<UserContext>
    {
        readonly PositionPlacementInstruction _startPosition;
        readonly PositionPlacementInstruction _sleepingPosition;
        readonly PositionPlacementInstruction _endPosition;
        readonly ISleepAnimations _sleepAnimations;

        UserContext _userContext;

        public Bed(
            string name,
            PositionPlacementInstruction startPosition,
            PositionPlacementInstruction sleepingPosition,
            PositionPlacementInstruction endPosition,
            ISleepAnimations sleepAnimations,
            ILogger logger) : base(logger)
        {
            _startPosition = startPosition;
            _sleepingPosition = sleepingPosition;
            _endPosition = endPosition;
            _sleepAnimations = sleepAnimations;

            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse(UserContext userContext)
        {
            _userContext = userContext;

            _userContext.PositionPlacementController.PlaceAt(
                _startPosition,
                onCompleted: () =>
                {
                    _userContext.SleepController.WokenUp += OnWokenUp;
                    _userContext.SleepController.GoToSleep(_sleepAnimations);

                    _userContext.PositionPlacementController.PlaceAt(_sleepingPosition);
                });
        }

        protected override void OnEndUse()
        {
            _userContext.PositionPlacementController.PlaceAt(_endPosition);
            _userContext.SleepController.WakeUp();
        }

        void OnWokenUp()
        {
            _userContext.SleepController.WokenUp -= OnWokenUp;
            _userContext.PositionPlacementController.PlaceAt(_endPosition, Ended);
        }
    }
}