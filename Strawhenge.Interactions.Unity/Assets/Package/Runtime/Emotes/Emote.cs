using FunctionalUtilities;
using Strawhenge.Inventory.Items;

namespace Strawhenge.Interactions.Unity.Emotes
{
    class Emote : OneAtATime
    {
        readonly EmoteAnimationHandler _animationHandler;
        readonly Maybe<BarkController> _barkController;
        readonly Maybe<Inventory.Inventory> _inventory;
        readonly EmoteScriptableObject _emote;

        InventoryItem _item;

        public Emote(EmoteAnimationHandler animationHandler,
            Maybe<BarkController> barkController,
            Maybe<Inventory.Inventory> inventory,
            EmoteScriptableObject emote)
        {
            _animationHandler = animationHandler;
            _barkController = barkController;
            _inventory = inventory;
            _emote = emote;
        }

        internal EmoteScriptableObject EmoteScriptableObject => _emote;

        protected override void OnStart()
        {
            _animationHandler.AnimationEnded += OnAnimationEnded;

            if (HasItem(out _item))
            {
                _item.HoldRightHand(BeginEmote);
                return;
            }

            BeginEmote();
        }

        protected override void OnStopRequested()
        {
            _animationHandler.End();
        }

        void BeginEmote()
        {
            _animationHandler.Perform(
                _emote.Animation,
                _emote.IsRepeating,
                _emote.LayerId,
                _emote.AnimatorBoolParameters);

            _emote.Bark.Do(
                bark => _barkController.Do(
                    barkController => barkController.Play(bark)));
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