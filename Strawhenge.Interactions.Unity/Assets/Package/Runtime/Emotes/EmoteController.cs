using FunctionalUtilities;
using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteController
    {
        readonly OneAtATimeManager<Emote> _oneAtATimeManager;
        readonly EmoteAnimationHandler _animationHandler;
        readonly Maybe<BarkController> _barkController;
        readonly Maybe<Inventory.Inventory> _inventory;

        public EmoteController(
            Animator animator,
            Maybe<BarkController> barkController,
            Maybe<Inventory.Inventory> inventory,
            ILogger logger)
        {
            _oneAtATimeManager = new OneAtATimeManager<Emote>();
            _oneAtATimeManager.Started += () => EmoteBegan?.Invoke();
            _oneAtATimeManager.Stopped += () => EmoteEnded?.Invoke();

            _animationHandler = new EmoteAnimationHandler(animator, logger);
            _barkController = barkController;
            _inventory = inventory;
        }

        public event Action EmoteBegan;

        public event Action EmoteEnded;

        public bool IsPerformingEmote => _oneAtATimeManager.Current.HasSome();

        public Maybe<EmoteScriptableObject> Current =>
            _oneAtATimeManager.Current.Map(x => x.EmoteScriptableObject);

        public void Perform(EmoteScriptableObject emote, Action callback = null)
        {
            _oneAtATimeManager.Start(
                new Emote(_animationHandler, _barkController, _inventory, emote),
                callback);
        }

        public void End()
        {
            _oneAtATimeManager.Stop();
        }
    }
}