using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class ChairScript : FurnitureScript
    {
        [SerializeField] SerializedPositionPlacement _startPosition;
        [SerializeField] SerializedPositionPlacement _sittingPosition;
        [SerializeField] SerializedPositionPlacement _endPosition;
        [SerializeField] SitTypeScriptableObject _sitType;
        [SerializeField] LoggerScript _logger;

        Chair _chair;

        public override Interactions.Furniture.Furniture Furniture => _chair ??= CreateChair();

        void Awake()
        {
            _chair ??= CreateChair();
        }

        Chair CreateChair()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            if (_sitType == null)
                logger.LogError($"'{nameof(_sitType)}' not set.");

            var startPosition = new PositionPlacementInstruction(
                _startPosition.Target.Reduce(() => transform),
                _startPosition.Args);

            var sittingPosition = new PositionPlacementInstruction(
                _sittingPosition.Target.Reduce(() => transform),
                _sittingPosition.Args);

            var endPosition = new PositionPlacementInstruction(
                _endPosition.Target.Reduce(() => transform),
                _endPosition.Args);

            return new Chair(
                name,
                startPosition,
                sittingPosition,
                endPosition,
                _sitType,
                logger);
        }
    }
}