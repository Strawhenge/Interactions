using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Emotes
{
    class EmoteAnimationHandler
    {
        readonly Animator _animator;
        readonly ILogger _logger;
        readonly StateMachineEvents<EmotesStateMachine> _stateMachineEvents;

        public EmoteAnimationHandler(Animator animator, ILogger logger)
        {
            _animator = animator;
            _logger = logger;

            _stateMachineEvents = _animator.AddEvents<EmotesStateMachine>(
                stateMachine =>
                {
                    stateMachine.OnEmoteEnded = OnAnimationEnded;
                    stateMachine.Destroyed += OnAnimationEnded;
                },
                stateMachine => stateMachine.Destroyed -= OnAnimationEnded);
        }

        public event Action AnimationEnded;

        public void Perform(int emoteId, bool useRootMotion)
        {
            if (!_animator.isActiveAndEnabled)
            {
                _logger.LogWarning("Animator is not active.");
                AnimationEnded?.Invoke();
            }

            _stateMachineEvents.PrepareIfRequired();

            _animator.SetInteger(AnimatorParameters.EmoteId, emoteId);
            _animator.SetTrigger(AnimatorParameters.BeginEmote);
            _animator.applyRootMotion = useRootMotion;
        }

        public void End()
        {
            _stateMachineEvents.PrepareIfRequired();
            _animator.SetTrigger(AnimatorParameters.EndEmote);
        }

        void OnAnimationEnded()
        {
            if (_animator != null)
            {
                _animator.applyRootMotion = false;
                _animator.ResetTrigger(AnimatorParameters.BeginEmote);
                _animator.ResetTrigger(AnimatorParameters.EndEmote);
            }

            AnimationEnded?.Invoke();
        }
    }
}