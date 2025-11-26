using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepController
    {
        readonly SleepAnimationHandler _animationHandler;
        readonly SleepTypeScriptableObject _defaultSleepType;
        readonly ILogger _logger;

        public SleepController(
            Animator animator,
            SleepTypeScriptableObject defaultSleepType,
            ILogger logger)
        {
            _animationHandler = new SleepAnimationHandler(animator, logger);
            _animationHandler.Sleeping += OnSleeping;
            _animationHandler.WokenUp += OnWokenUp;

            _defaultSleepType = defaultSleepType;
            _logger = logger;
        }

        public event Action GoingToSleep;

        public event Action Sleeping;

        public event Action WakingUp;

        public event Action WokenUp;

        public SleepState State { get; private set; } = SleepState.Awake;

        public bool IsSleepInProgress => State != SleepState.Awake;

        public void GoToSleep(SleepTypeScriptableObject sleepType = null)
        {
            if (IsSleepInProgress)
            {
                _logger.LogWarning($"Sleep is already in progress.");
                return;
            }

            ChangeState(SleepState.GoingToSleep, GoingToSleep);
            _animationHandler.GoToSleep(sleepType ?? _defaultSleepType);
        }

        public void WakeUp()
        {
            if (!IsSleepInProgress)
                return;

            ChangeState(SleepState.WakingUp, WakingUp);
            _animationHandler.WakeUp();
        }

        void OnSleeping() => ChangeState(SleepState.Sleeping, Sleeping);

        void OnWokenUp() => ChangeState(SleepState.Awake, WokenUp);

        void ChangeState(SleepState newState, Action eventToInvoke)
        {
            if (newState == State)
                return;

            State = newState;
            eventToInvoke?.Invoke();
        }
    }
}