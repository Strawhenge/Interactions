using FunctionalUtilities;
using Strawhenge.Common.Unity.AnimatorBehaviours;
using UnityEngine;
using System;

namespace Strawhenge.Interactions.Unity.Emotes
{
    class EmoteAnimationHandler
    {
        static readonly int BeginEmote = Animator.StringToHash("Begin Emote");
        static readonly int EndEmote = Animator.StringToHash("End Emote");

        readonly Animator _animator;
        readonly AnimatorOverrideController _animatorOverrideController;
        readonly StateMachineEvents<EmotesStateMachine> _stateMachineEvents;

        public EmoteAnimationHandler(Animator animator)
        {
            _animator = animator;
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;

            _stateMachineEvents = _animator.AddEvents<EmotesStateMachine>(
                stateMachine => stateMachine.OnEmoteEnded = OnAnimationEnded,
                _ => { });
        }

        public event Action AnimationEnded;

        public void Perform(Maybe<AnimationClip> animation)
        {
            _stateMachineEvents.PrepareIfRequired();

            animation.Do(a => _animatorOverrideController["Emote"] = a);
            _animator.SetTrigger(BeginEmote);
        }

        public void End()
        {
            _stateMachineEvents.PrepareIfRequired();
            _animator.SetTrigger(EndEmote);
        }

        void OnAnimationEnded()
        {
            _animator.ResetTrigger(BeginEmote);
            _animator.ResetTrigger(EndEmote);
            AnimationEnded?.Invoke();
        }
    }
}