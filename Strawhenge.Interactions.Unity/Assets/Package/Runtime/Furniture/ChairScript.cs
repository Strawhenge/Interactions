using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Serialization;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using Strawhenge.Interactions.Unity.Sit;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class ChairScript : FurnitureScript
    {
        [SerializeField] Transform _startPosition;
        [SerializeField] Transform _sittingPosition;
        [SerializeField] Transform _endPosition;

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

            return new Chair(name, logger);
        }
    }
}