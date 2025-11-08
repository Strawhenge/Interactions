using FunctionalUtilities;
using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteController
    {
        readonly OneAtATimeManager<Emote> _oneAtATimeManager = new();
        readonly EmoteAnimationHandler _animationHandler;
        readonly Maybe<Inventory.Inventory> _inventory;

        public EmoteController(
            Animator animator,
            Maybe<Inventory.Inventory> inventory,
            ILogger logger)
        {
            _animationHandler = new EmoteAnimationHandler(animator, logger);
            _inventory = inventory;
        }

        public bool IsPerformingEmote => _oneAtATimeManager.Current.HasSome();

        public Maybe<EmoteScriptableObject> Current =>
            _oneAtATimeManager.Current.Map(x => x.EmoteScriptableObject);

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