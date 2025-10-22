using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitController
    {
        readonly SitAnimationHandler _animationHandler;
        readonly ISitAnimations _defaultAnimations;

        public SitController(Animator animator, ISitAnimations defaultAnimations)
        {
            _animationHandler = new SitAnimationHandler(animator);
            _defaultAnimations = defaultAnimations;
        }

        public bool IsSitting { get; private set; }

        public event Action Sitting;

        public event Action Standing;

        public void Sit(ISitAnimations animations = null)
        {
            if (IsSitting) return;
            IsSitting = true;

            _animationHandler.Sitting += OnSitting;
            _animationHandler.Sit(animations ?? _defaultAnimations);
        }

        public void Stand()
        {
            if (!IsSitting) return;

            _animationHandler.Standing += OnStanding;
            _animationHandler.Stand();
        }

        void OnSitting()
        {
            _animationHandler.Sitting -= OnSitting;
            Sitting?.Invoke();
        }

        void OnStanding()
        {
            _animationHandler.Standing -= OnStanding;
            IsSitting = false;
            Standing?.Invoke();
        }
    }
}