using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class EmoteFurnitureScript : FurnitureScript
    {
        [SerializeField] EmoteScriptableObject _emote;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        Furniture<UserContext> _furniture;

        public override Furniture<UserContext> Furniture => _furniture ??= CreateFurniture();

        void Awake()
        {
            _furniture ??= CreateFurniture();
        }

        Furniture<UserContext> CreateFurniture()
        {
            if (_emote == null)
            {
                Debug.LogError($"'{nameof(_emote)}' not set.");
                return NullFurniture<UserContext>.Instance;
            }

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new EmoteFurniture(name, _emote, logger);
        }
    }
}