using FunctionalUtilities;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Inventory.Unity;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField, Tooltip("Optional.")] InventoryScript _inventory;

        EmoteController _emoteController;

        public EmoteController EmoteController => _emoteController ??= CreateEmoteController();

        void Awake()
        {
            _emoteController ??= CreateEmoteController();
        }

        EmoteController CreateEmoteController()
        {
            // TODO New helper to check root object.
            ComponentRefHelper.EnsureHierarchyComponent(ref _animator, nameof(_animator), this);

            return new EmoteController(
                _animator,
                Maybe.NotNull(_inventory).Map(i => i.Inventory));
        }
    }
}