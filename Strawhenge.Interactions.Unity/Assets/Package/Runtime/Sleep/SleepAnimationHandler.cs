using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    class SleepAnimationHandler
    {
        readonly Animator _animator;
        readonly AnimatorOverrideController _overrideController;
        readonly StateMachineEvents<SleepStateMachine> _stateMachineEvents;

        public SleepAnimationHandler(Animator animator)
        {
            _animator = animator;
            _overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

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

        public void GoToSleep(ISleepAnimations sleepAnimations)
        {
            _stateMachineEvents.PrepareIfRequired();

            _overrideController[PlaceholderAnimationClips.LayDown] = sleepAnimations.LayDown;
            _overrideController[PlaceholderAnimationClips.Sleeping] = sleepAnimations.Sleeping;
            _overrideController[PlaceholderAnimationClips.GetUp] = sleepAnimations.GetUp;
            _animator.runtimeAnimatorController = _overrideController;

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