using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Sleep
{
    class SleepAnimationHandler
    {
        readonly Animator _animator;
        readonly StateMachineEvents<SleepStateMachine> _stateMachineEvents;
        readonly ILogger _logger;

        public SleepAnimationHandler(Animator animator, ILogger logger)
        {
            _animator = animator;

            _stateMachineEvents = animator.AddEvents<SleepStateMachine>(
                subscribe: stateMachine =>
                {
                    stateMachine.OnSleeping = OnSleeping;
                    stateMachine.OnWokenUp = OnWokenUp;
                    stateMachine.Destroyed += OnStateMachineDestroyed;
                },
                unsubscribe: stateMachine => stateMachine.Destroyed -= OnStateMachineDestroyed);

            _logger = logger;
        }

        public event Action Sleeping;

        public event Action WokenUp;

        public void GoToSleep(SleepTypeScriptableObject sleepType)
        {
            _stateMachineEvents.PrepareIfRequired();

            _animator.SetInteger(AnimatorParameters.SleepTypeId.Id, sleepType.Id);
            _animator.SetTrigger(AnimatorParameters.Sleep.Id);
        }

        public void WakeUp()
        {
            _stateMachineEvents.PrepareIfRequired();

            _animator.SetTrigger(AnimatorParameters.WakeUp.Id);
        }

        void OnSleeping() => Sleeping?.Invoke();

        void OnWokenUp() => WokenUp?.Invoke();

        void OnStateMachineDestroyed()
        {
            _logger.LogInformation($"'{nameof(SleepStateMachine)}' destroyed.");
            WokenUp?.Invoke();
        }
    }
}