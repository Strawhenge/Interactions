using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitController
    {
        readonly SitAnimationHandler _animationHandler;
        readonly SitTypeScriptableObject _defaultSitType;

        public SitController(Animator animator, SitTypeScriptableObject defaultSitType)
        {
            _animationHandler = new SitAnimationHandler(animator);
            _defaultSitType = defaultSitType;
        }

        public bool IsSitting { get; private set; }

        public event Action Sitting;

        public event Action Standing;

        public void Sit(SitTypeScriptableObject sitType = null)
        {
            if (IsSitting) return;
            IsSitting = true;

            _animationHandler.Sitting += OnSitting;
            _animationHandler.Sit(sitType ?? _defaultSitType);
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