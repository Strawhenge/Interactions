using FunctionalUtilities;
using Strawhenge.Interactions.OneAtATime;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteController
    {
        readonly OneAtATimeManager _oneAtATimeManager = new();
        readonly EmoteAnimationHandler _animationHandler;
        readonly Maybe<Inventory.Inventory> _inventory;

        public EmoteController(Animator animator, Maybe<Inventory.Inventory> inventory)
        {
            _animationHandler = new EmoteAnimationHandler(animator);
            _inventory = inventory;
        }

        public void Perform(EmoteScriptableObject emote, Action callback = null)
        {
            _oneAtATimeManager.Start(
                new Emote(_animationHandler, _inventory, emote),
                callback);
        }

        public void End()
        {
            _oneAtATimeManager.Stop();
        }
    }
}