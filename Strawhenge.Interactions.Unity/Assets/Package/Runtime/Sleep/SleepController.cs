using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepController
    {
        readonly SleepAnimationHandler _animationHandler;
        readonly SleepTypeScriptableObject _defaultSleepType;

        public SleepController(Animator animator, SleepTypeScriptableObject defaultSleepType)
        {
            _animationHandler = new SleepAnimationHandler(animator);
            _animationHandler.Sleeping += OnSleeping;
            _animationHandler.WokenUp += OnWokenUp;

            _defaultSleepType = defaultSleepType;
        }

        public event Action GoingToSleep;

        public event Action Sleeping;

        public event Action WakingUp;

        public event Action WokenUp;

        public bool IsGoingToSleep { get; private set; }

        public bool IsSleeping { get; private set; }

        public bool IsWakingUp { get; private set; }

        public void GoToSleep(SleepTypeScriptableObject sleepType = null)
        {
            if (IsGoingToSleep || IsSleeping)
                return;

            IsGoingToSleep = true;
            _animationHandler.GoToSleep(sleepType ?? _defaultSleepType);
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