using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Emotes;
using Strawhenge.Interactions.Unity.PositionPlacement;
using Strawhenge.Interactions.Unity.Sit;
using Strawhenge.Interactions.Unity.Sleep;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Furniture
{
    public class FurnitureUserScript : MonoBehaviour
    {
        [SerializeField] InteractionsContextScript _context;
        [SerializeField] EmotesScript _emotes;
        [SerializeField] SitScript _sit;
        [SerializeField] SleepScript _sleep;
        [SerializeField] PositionPlacementScript _positionPlacement;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        FurnitureUser _user;

        public FurnitureUser User => _user ??= CreateUser();

        void Awake()
        {
            _user ??= CreateUser();
        }

        FurnitureUser CreateUser()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _context, nameof(_context), this);
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _emotes, nameof(_emotes), this);
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _sit, nameof(_sit), this);
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _sleep, nameof(_sleep), this);
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _positionPlacement, nameof(_positionPlacement), this);

            var userScope = new FurnitureUserScope(
                _emotes.EmoteController,
                _sit.SitController,
                _sleep.SleepController,
                _positionPlacement.PositionPlacementController);

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new FurnitureUser(userScope, _context.Context, logger);
        }
    }
}