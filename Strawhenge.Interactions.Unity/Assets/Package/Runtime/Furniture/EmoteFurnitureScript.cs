using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurnitureScript : FurnitureScript
    {
        [SerializeField] EmoteScriptableObject _emote;
        [SerializeField] SerializedPositionPlacement _positionPlacement;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        Interactions.Furniture.Furniture _furniture;

        public override Interactions.Furniture.Furniture Furniture => _furniture ??= CreateFurniture();

        void Awake()
        {
            _furniture ??= CreateFurniture();
        }

        Interactions.Furniture.Furniture CreateFurniture()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            if (_emote == null)
            {
                logger.LogError($"'{nameof(_emote)}' not set.");
                return NullFurniture.Instance;
            }

            var positionPlacement = new PositionPlacementInstruction(
                _positionPlacement.Target.Reduce(() => transform),
                _positionPlacement.Args);

            return new EmoteFurniture(
                name,
                _emote,
                positionPlacement,
                logger);
        }
    }
}