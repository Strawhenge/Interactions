using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    class SleepAnimationHandler
    {
        readonly Animator _animator;
        readonly StateMachineEvents<SleepStateMachine> _stateMachineEvents;

        public SleepAnimationHandler(Animator animator)
        {
            _animator = animator;

            _stateMachineEvents = animator.AddEvents<SleepStateMachine>(
                subscribe: stateMachine =>
                {
                    stateMachine.OnSleeping = OnSleeping;
                    stateMachine.OnWokenUp = OnWokenUp;
                },
                unsubscribe: _ => { });
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
    }
}