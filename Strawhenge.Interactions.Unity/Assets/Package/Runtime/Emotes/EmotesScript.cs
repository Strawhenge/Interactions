using FunctionalUtilities;
using Strawhenge.Common.Unity.AnimatorBehaviours;
using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Interactions.OneAtATime;
using Strawhenge.Inventory.Items;
using Strawhenge.Inventory.Unity;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField, Tooltip("Optional.")] InventoryScript _inventory;

        OneAtATimeManager _oneAtATimeManager = new();
        EmoteAnimationHandler _animationHandler;
        InventoryItem _item;

        void Awake()
        {
            // TODO ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);
            ComponentRefHelper.EnsureHierarchyComponent(ref _animator, nameof(_animator), this);

            _animationHandler = new EmoteAnimationHandler(_animator);
            //_animationHandler.AnimationEnded += OnAnimationEnded;
        }

        public void Perform(EmoteScriptableObject emote)
        {
            _oneAtATimeManager.Start(
                new Emote(_animationHandler, Maybe.NotNull(_inventory).Map(x => x.Inventory), emote),
                () => Debug.Log("Ended " + emote.name));

            // if (emote.Item.HasSome(out var item))
            // {
            //     _item = _inventory.Inventory
            //         .GetItemOrCreateTemporary(item.ToItem());
            //     _item.HoldRightHand(() => _animationHandler.Perform(emote.Animation));
            //     return;
            // }

            //_animationHandler.Perform(emote.Animation);
        }

        public void End()
        {
            _oneAtATimeManager.Stop();
            //_animationHandler.End();
        }

        // void OnAnimationEnded()
        // {
        //     _item?.ClearFromHands();
        //     _item = null;
        // }
    }
}