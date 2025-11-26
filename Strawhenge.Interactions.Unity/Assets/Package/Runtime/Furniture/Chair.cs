using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity
{
    public class Chair : Interactions.Furniture.Furniture
    {
        readonly PositionPlacementInstruction _startPosition;
        readonly PositionPlacementInstruction _sittingPosition;
        readonly PositionPlacementInstruction _endPosition;
        readonly SitTypeScriptableObject _sitType;
        readonly ILogger _logger;

        PositionPlacementController _positionPlacementController;
        SitController _sitController;

        public Chair(
            string name,
            PositionPlacementInstruction startPosition,
            PositionPlacementInstruction sittingPosition,
            PositionPlacementInstruction endPosition,
            SitTypeScriptableObject sitType,
            ILogger logger) : base(logger)
        {
            _startPosition = startPosition;
            _sittingPosition = sittingPosition;
            _endPosition = endPosition;
            _sitType = sitType;
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

            if (!userScope.Get<SitController>().HasSome(out _sitController))
            {
                _logger.LogWarning($"'{nameof(SitController)}' not found in user scope.");
                Ended();
                return;
            }

            _positionPlacementController.PlaceAt(
                _startPosition,
                onCompleted: () =>
                {
                    _sitController.Standing += OnStanding;
                    _sitController.Sit(_sitType);

                    _positionPlacementController.PlaceAt(_sittingPosition);
                });
        }

        protected override void OnEndUse()
        {
            _positionPlacementController.PlaceAt(_endPosition);
            _sitController.Stand();
        }

        void OnStanding()
        {
            _sitController.Standing -= OnStanding;
            _positionPlacementController.PlaceAt(_endPosition, Ended);

            _positionPlacementController = null;
            _sitController = null;
        }
    }
}