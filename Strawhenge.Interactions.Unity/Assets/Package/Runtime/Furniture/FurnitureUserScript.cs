using FunctionalUtilities;
using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class FurnitureUserScript : MonoBehaviour
    {
        [SerializeField] EmotesScript _emotes;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        FurnitureUser<UserContext> _user;

        public FurnitureUser<UserContext> User => _user ??= CreateUser();

        void Awake()
        {
            _user ??= CreateUser();
        }

        FurnitureUser<UserContext> CreateUser()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _emotes, nameof(_emotes), this);

            var context = new UserContext(_emotes.EmoteController);

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new FurnitureUser<UserContext>(context, logger);
        }
    }
}