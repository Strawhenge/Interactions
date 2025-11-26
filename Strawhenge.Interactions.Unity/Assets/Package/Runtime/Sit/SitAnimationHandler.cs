using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Sit
{
    class SitAnimationHandler
    {
        readonly Animator _animator;
        readonly StateMachineEvents<SitStateMachine> _stateMachineEvents;
        readonly ILogger _logger;

        public event Action Sitting;

        public event Action Standing;

        public SitAnimationHandler(Animator animator, ILogger logger)
        {
            _animator = animator;

            _stateMachineEvents = animator.AddEvents<SitStateMachine>(
                subscribe: stateMachine =>
                {
                    stateMachine.OnSitting = OnSitting;
                    stateMachine.OnStanding = OnStanding;
                    stateMachine.Destroyed += OnStateMachineDestroyed;
                },
                unsubscribe: stateMachine => stateMachine.Destroyed -= OnStateMachineDestroyed);

            _logger = logger;
        }

        public void Sit(SitTypeScriptableObject sitType)
        {
            if (!_animator.isActiveAndEnabled)
            {
                _logger.LogWarning("Animator is not active.");
                Standing?.Invoke();
            }

            _stateMachineEvents.PrepareIfRequired();

            _animator.ResetTrigger(AnimatorParameters.Stand.Id);
            _animator.ResetTrigger(AnimatorParameters.Sit.Id);

            _animator.SetInteger(AnimatorParameters.SitTypeId.Id, sitType.Id);
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

        void OnStateMachineDestroyed()
        {
            _logger.LogInformation($"'{nameof(SitStateMachine)}' destroyed.");
            OnStanding();
        }
    }
}