using FunctionalUtilities;
using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class FurnitureUserScript : MonoBehaviour
    {
        [SerializeField] EmotesScript _emotes;
        [SerializeField] PositionPlacementScript _positionPlacement;
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
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _positionPlacement, nameof(_positionPlacement), this);

            var context = new UserContext(_emotes.EmoteController, _positionPlacement.PositionPlacementController);

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new FurnitureUser<UserContext>(context, logger);
        }
    }
}