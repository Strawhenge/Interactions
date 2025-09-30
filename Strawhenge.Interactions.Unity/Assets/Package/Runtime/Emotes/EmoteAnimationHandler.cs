using FunctionalUtilities;
using Strawhenge.Common;
using Strawhenge.Common.Unity.AnimatorBehaviours;
using Strawhenge.Interactions.Unity.Package.Runtime;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Strawhenge.Interactions.Unity.Emotes
{
    class EmoteAnimationHandler
    {
        readonly Animator _animator;
        readonly AnimatorOverrideController _animatorOverrideController;
        readonly StateMachineEvents<EmotesStateMachine> _stateMachineEvents;

        int[] _boolParameters = Array.Empty<int>();

        public EmoteAnimationHandler(Animator animator)
        {
            _animator = animator;
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;

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
            _stateMachineEvents.PrepareIfRequired();

            animation.Do(a => _animatorOverrideController["Emote"] = a);

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
            _boolParameters.ForEach(id => _animator.SetBool(id, false));
            _boolParameters = Array.Empty<int>();
            _animator.ResetTrigger(AnimatorParameters.BeginEmote);
            _animator.ResetTrigger(AnimatorParameters.EndEmote);
            AnimationEnded?.Invoke();
        }
    }
}