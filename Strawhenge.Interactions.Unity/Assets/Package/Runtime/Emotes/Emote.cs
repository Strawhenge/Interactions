using FunctionalUtilities;
using Strawhenge.Inventory.Items;
using System;

namespace Strawhenge.Interactions.Unity.Emotes
{
    class Emote : OneAtATime.OneAtATime
    {
        readonly EmoteAnimationHandler _animationHandler;
        readonly Maybe<Inventory.Inventory> _inventory;
        readonly EmoteScriptableObject _emote;

        InventoryItem _item;
        bool _animationStarted;

        public Emote(
            EmoteAnimationHandler animationHandler,
            Maybe<Inventory.Inventory> inventory,
            EmoteScriptableObject emote)
        {
            _animationHandler = animationHandler;
            _inventory = inventory;
            _emote = emote;
        }

        protected override void OnStart()
        {
            if (HasItem(out _item))
            {
                _item.HoldRightHand(StartAnimation);
                return;
            }

            StartAnimation();
        }

        void StartAnimation()
        {
            _animationStarted = true;
            _animationHandler.AnimationEnded += OnAnimationEnded;
            _animationHandler.Perform(_emote.Animation);
        }

        void OnAnimationEnded()
        {
            _animationHandler.AnimationEnded -= OnAnimationEnded;

            if (_item != null)
            {
                _item.ClearFromHands(InvokeStopped);
                return;
            }

            InvokeStopped();
        }

        protected override void OnStop(Action onStopped)
        {
            if (!_animationStarted)
            {
                if (_item != null)
                    _item.ClearFromHands(onStopped);
                else
                    onStopped();

                return;
            }

            _animationHandler.AnimationEnded -= OnAnimationEnded;
            _animationHandler.End();

            if (_item != null)
                _item.ClearFromHands(onStopped);
            else
                onStopped();
        }

        bool HasItem(out InventoryItem item)
        {
            return _emote.Item
                .Map(i => _inventory
                    .Map(inventory => inventory.GetItemOrCreateTemporary(i.ToItem())))
                .Flatten()
                .HasSome(out item);
        }
    }
}