using FunctionalUtilities;
using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Inventory.Unity;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField, Tooltip("Optional.")] BarkScript _barks;
        [SerializeField, Tooltip("Optional.")] InventoryScript _inventory;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        EmoteController _emoteController;

        public EmoteController EmoteController => _emoteController ??= CreateEmoteController();

        void Awake()
        {
            _emoteController ??= CreateEmoteController();
        }

        EmoteController CreateEmoteController()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            var barks = Maybe
                .NotNull(_barks)
                .Map(b => b.BarkController);

            var inventory = Maybe
                .NotNull(_inventory)
                .Map(i => i.Inventory);

            return new EmoteController(_animator, barks, inventory, logger);
        }
    }
}