﻿using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitController
    {
        readonly SitAnimationHandler _animationHandler;

        public SitController(Animator animator)
        {
            _animationHandler = new SitAnimationHandler(animator);
        }

        public bool IsSitting { get; private set; }

        public event Action Sitting;

        public event Action Standing;

        public void Sit()
        {
            if (IsSitting) return;
            IsSitting = true;

            _animationHandler.Sitting += OnSitting;
            _animationHandler.Sit();
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