using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Serialization;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurnitureScript : FurnitureScript
    {
        [SerializeField] EmoteScriptableObject _emote;

        [SerializeField] SerializedSource<
            IPositionPlacementArgs,
            SerializedPositionPlacementArgs,
            PositionPlacementArgsScriptableObject> _positionPlacementArgs;

        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        Furniture<UserContext> _furniture;

        public override Furniture<UserContext> Furniture => _furniture ??= CreateFurniture();

        void Awake()
        {
            _furniture ??= CreateFurniture();
        }

        Furniture<UserContext> CreateFurniture()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            if (_emote == null)
            {
                logger.LogError($"'{nameof(_emote)}' not set.");
                return NullFurniture<UserContext>.Instance;
            }

            return new EmoteFurniture(name, _emote, transform, _positionPlacementArgs.GetValue(), logger);
        }
    }
}