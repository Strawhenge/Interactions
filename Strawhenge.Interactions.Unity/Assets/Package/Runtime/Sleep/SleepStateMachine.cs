using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepStateMachine : StateMachineBehaviour, IHasDestroyedEvent
    {
        const string SleepingStateName = "Sleeping";
        const string WakeUpStateName = "WakeUp";

        public event Action Destroyed;

        internal Action OnSleeping { private get; set; } = () => { };

        internal Action OnWokenUp { private get; set; } = () => { };

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName(SleepingStateName))
            {
                OnSleeping();
            }

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName(WakeUpStateName))
            {
                OnWokenUp();
            }

            base.OnStateExit(animator, stateInfo, layerIndex);
        }

        void OnDestroy() => Destroyed?.Invoke();
    }
}