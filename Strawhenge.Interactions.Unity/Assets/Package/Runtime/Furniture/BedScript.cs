using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sleep;
using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class BedScript : FurnitureScript
    {
        [SerializeField] SerializedPositionPlacement _startPosition;
        [SerializeField] SerializedPositionPlacement _sleepingPosition;
        [SerializeField] SerializedPositionPlacement _endPosition;
        [SerializeField] SleepTypeScriptableObject _sleepType;
        [SerializeField] LoggerScript _logger;

        Bed _bed;

        public override Interactions.Furniture.Furniture Furniture => _bed ??= CreateBed();

        void Awake()
        {
            _bed ??= CreateBed();
        }

        Bed CreateBed()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            var startPosition = new PositionPlacementInstruction(
                _startPosition.Target.Reduce(() => transform),
                _startPosition.Args);

            var sleepingPosition = new PositionPlacementInstruction(
                _sleepingPosition.Target.Reduce(() => transform),
                _sleepingPosition.Args);

            var endPosition = new PositionPlacementInstruction(
                _endPosition.Target.Reduce(() => transform),
                _endPosition.Args);

            return new Bed(
                name,
                startPosition,
                sleepingPosition,
                endPosition,
                _sleepType, // TODO Handle missing case
                logger);
        }
    }
}