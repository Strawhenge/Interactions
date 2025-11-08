using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sleep;

namespace Strawhenge.Interactions.Unity
{
    public class Bed : Interactions.Furniture.Furniture
    {
        readonly PositionPlacementInstruction _startPosition;
        readonly PositionPlacementInstruction _sleepingPosition;
        readonly PositionPlacementInstruction _endPosition;
        readonly ISleepAnimations _sleepAnimations;
        readonly ILogger _logger;

        PositionPlacementController _positionPlacementController;
        SleepController _sleepController;

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
            _logger = logger;

            Name = name;
        }

        public override string Name { get; }

        protected override void OnUse(IFurnitureUserScope userScope)
        {
            if (!userScope.Get<PositionPlacementController>().HasSome(out _positionPlacementController))
            {
                _logger.LogWarning($"'{nameof(PositionPlacementController)}' not found in user scope.");
                Ended();
                return;
            }

            if (!userScope.Get<SleepController>().HasSome(out _sleepController))
            {
                _logger.LogWarning($"'{nameof(SleepController)}' not found in user scope.");
                Ended();
                return;
            }

            _positionPlacementController.PlaceAt(
                _startPosition,
                onCompleted: () =>
                {
                    _sleepController.WokenUp += OnWokenUp;
                    _sleepController.GoToSleep(_sleepAnimations);

                    _positionPlacementController.PlaceAt(_sleepingPosition);
                });
        }

        protected override void OnEndUse()
        {
            _positionPlacementController?.PlaceAt(_endPosition);
            _sleepController?.WakeUp();

            _positionPlacementController = null;
            _sleepController = null;
        }

        void OnWokenUp()
        {
            _sleepController.WokenUp -= OnWokenUp;
            _positionPlacementController.PlaceAt(_endPosition, Ended);
        }
    }
}