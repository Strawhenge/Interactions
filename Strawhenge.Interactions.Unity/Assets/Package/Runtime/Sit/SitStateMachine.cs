using Strawhenge.Common.Unity.AnimatorBehaviours;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitStateMachine : StateMachineBehaviour, IHasDestroyedEvent
    {
        const string SittingStateName = "Sitting";
        const string StandStateName = "Stand";

        public Action OnSitting = () => { };
        public Action OnStanding = () => { };

        public event Action Destroyed;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (IsSitting(stateInfo))
            {
                OnSitting();
            }

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (IsStanding(stateInfo))
            {
                OnStanding();
            }

            base.OnStateExit(animator, stateInfo, layerIndex);
        }

        static bool IsSitting(AnimatorStateInfo stateInfo) => stateInfo.IsName(SittingStateName);

        static bool IsStanding(AnimatorStateInfo stateInfo) => stateInfo.IsName(StandStateName);

        void OnDestroy() => Destroyed?.Invoke();
    }
}