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

        public Emote(
            EmoteAnimationHandler animationHandler,
            Maybe<Inventory.Inventory> inventory,
            EmoteScriptableObject emote)
        {
            _animationHandler = animationHandler;
            _inventory = inventory;
            _inventory.Do(i => i.Hands.RightHand.Changed += OnHandChanged);
            _emote = emote;
        }

        void OnHandChanged()
        {
        }

        protected override void OnStart()
        {
            _animationHandler.AnimationEnded += OnAnimationEnded;

            if (HasItem(out _item))
            {
                _item.HoldRightHand(() => _animationHandler.Perform(_emote.Animation));
                return;
            }

            _animationHandler.Perform(_emote.Animation);
        }

        protected override void OnStop(Action onStopped)
        {
            _animationHandler.End();
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