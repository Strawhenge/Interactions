using FunctionalUtilities;
using Strawhenge.Common;
using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.AnimatorBehaviours;
using Strawhenge.Interactions.Unity.Package.Runtime;
using UnityEngine;
using System;
using System.Collections.Generic;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Emotes
{
    class EmoteAnimationHandler
    {
        readonly Animator _animator;
        readonly ILogger _logger;
        readonly AnimatorOverrideController _animatorOverrideController;
        readonly StateMachineEvents<EmotesStateMachine> _stateMachineEvents;

        int[] _boolParameters = Array.Empty<int>();

        public EmoteAnimationHandler(Animator animator, ILogger logger)
        {
            _animator = animator;
            _logger = logger;
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

            _stateMachineEvents = _animator.AddEvents<EmotesStateMachine>(
                stateMachine =>
                {
                    stateMachine.OnEmoteEnded = OnAnimationEnded;
                    stateMachine.Destroyed += OnAnimationEnded;
                },
                stateMachine => stateMachine.Destroyed -= OnAnimationEnded);
        }

        public event Action AnimationEnded;

        public void Perform(
            Maybe<AnimationClip> animation,
            bool isRepeating,
            int layerId,
            IEnumerable<AnimatorBoolParameterScriptableObject> emoteAnimatorBoolParameters)
        {
            if (!_animator.isActiveAndEnabled)
            {
                _logger.LogWarning("Animator is not active.");
                AnimationEnded?.Invoke();
            }

            _stateMachineEvents.PrepareIfRequired();

            _animator.runtimeAnimatorController = _animatorOverrideController;
            animation.Do(a => _animatorOverrideController[PlaceholderAnimationClip.Name] = a);

            _animator.SetInteger(AnimatorParameters.EmoteLayerId, layerId);
            _animator.SetBool(AnimatorParameters.RepeatingEmote, isRepeating);

            _boolParameters = emoteAnimatorBoolParameters.ToArray(x => x.Id);
            _boolParameters.ForEach(id => _animator.SetBool(id, true));

            _animator.SetTrigger(AnimatorParameters.BeginEmote);
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
                _boolParameters.ForEach(id => _animator.SetBool(id, false));
                _boolParameters = Array.Empty<int>();
                _animator.ResetTrigger(AnimatorParameters.BeginEmote);
                _animator.ResetTrigger(AnimatorParameters.EndEmote);
            }

            AnimationEnded?.Invoke();
        }
    }
}