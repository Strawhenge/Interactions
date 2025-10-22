using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    class SitAnimationHandler
    {
        readonly Animator _animator;
        readonly AnimatorOverrideController _animatorOverrideController;
        readonly ISitAnimations _defaultAnimations;
        readonly StateMachineEvents<SitStateMachine> _stateMachineEvents;

        public event Action Sitting;
        public event Action Standing;

        public SitAnimationHandler(Animator animator, ISitAnimations defaultAnimations)
        {
            _animator = animator;
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

            _defaultAnimations = defaultAnimations;

            _stateMachineEvents = animator.AddEvents<SitStateMachine>(
                subscribe: stateMachine =>
                {
                    stateMachine.OnSitting = OnSitting;
                    stateMachine.OnStanding = OnStanding;
                },
                unsubscribe: _ => { });
        }

        public void Sit()
        {
            _stateMachineEvents.PrepareIfRequired();

            _animator.runtimeAnimatorController = _animatorOverrideController;
            _animatorOverrideController[PlaceholderAnimationClips.Sit] = _defaultAnimations.Sit;
            _animatorOverrideController[PlaceholderAnimationClips.Sitting] = _defaultAnimations.Sitting;
            _animatorOverrideController[PlaceholderAnimationClips.Stand] = _defaultAnimations.Stand;

            _animator.ResetTrigger(AnimatorParameters.Stand.Id);
            _animator.ResetTrigger(AnimatorParameters.Sit.Id);
            _animator.SetTrigger(AnimatorParameters.Sit.Id);
        }

        public void Stand()
        {
            _stateMachineEvents.PrepareIfRequired();

            _animator.ResetTrigger(AnimatorParameters.Stand.Id);
            _animator.SetTrigger(AnimatorParameters.Stand.Id);
        }

        void OnStanding() => Standing?.Invoke();

        void OnSitting() => Sitting?.Invoke();
    }
}