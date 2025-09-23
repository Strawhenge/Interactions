using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmotesStateMachine : StateMachineBehaviour, IHasDestroyedEvent
    {
        public event Action Destroyed;

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash) => OnEmoteEnded();

        internal Action OnEmoteEnded = () => { };

        void OnDestroy() => Destroyed?.Invoke();
    }
}