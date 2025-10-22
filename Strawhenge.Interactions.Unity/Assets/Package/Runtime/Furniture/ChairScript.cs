using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Serialization;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class ChairScript : FurnitureScript
    {
        [SerializeField] SerializedPositionPlacement _startPosition;
        [SerializeField] SerializedPositionPlacement _sittingPosition;
        [SerializeField] SerializedPositionPlacement _endPosition;

        [SerializeField] SerializedSource<ISitAnimations,
            SerializedSitAnimations,
            SitAnimationsScriptableObject> _sitAnimations;

        [SerializeField] LoggerScript _logger;

        Chair _chair;

        public override Furniture<UserContext> Furniture => _chair ??= CreateChair();

        void Awake()
        {
            _chair ??= CreateChair();
        }

        Chair CreateChair()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

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
                _sitAnimations.GetValue(),
                logger);
        }
    }
}