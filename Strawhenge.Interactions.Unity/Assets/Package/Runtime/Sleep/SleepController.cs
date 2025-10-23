using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepController
    {
        readonly SleepAnimationHandler _animationHandler;

        public SleepController(Animator animator)
        {
            _animationHandler = new SleepAnimationHandler(animator);
            _animationHandler.Sleeping += OnSleeping;
            _animationHandler.WokenUp += OnWokenUp;
        }

        public event Action GoingToSleep;

        public event Action Sleeping;

        public event Action WakingUp;

        public event Action WokenUp;

        public bool IsGoingToSleep { get; private set; }

        public bool IsSleeping { get; private set; }

        public bool IsWakingUp { get; private set; }

        public void GoToSleep()
        {
            if (IsGoingToSleep || IsSleeping)
                return;

            IsGoingToSleep = true;
            _animationHandler.GoToSleep();
            GoingToSleep?.Invoke();
        }

        public void WakeUp()
        {
            if (IsWakingUp || !(IsGoingToSleep || IsSleeping))
                return;

            IsWakingUp = true;
            _animationHandler.WakeUp();
            WakingUp?.Invoke();
        }

        void OnSleeping()
        {
            IsGoingToSleep = false;
            IsSleeping = true;
            IsWakingUp = false;
            Sleeping?.Invoke();
        }

        void OnWokenUp()
        {
            IsGoingToSleep = false;
            IsSleeping = false;
            IsWakingUp = false;
            WokenUp?.Invoke();
        }
    }
}