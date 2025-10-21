using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using System;
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
            // TODO Verify emote field is assigned, if not return null object.

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new EmoteFurniture(name, logger);
        }
    }
}