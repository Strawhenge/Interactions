using Strawhenge.Interactions.Unity.Sit;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class SitTypeHelper
    {
        public static void AddSitType(
            int id,
            AnimationClip sitAnimation,
            AnimationClip sittingAnimation,
            AnimationClip standAnimation,
            AnimatorStateMachine sitStateMachine,
            AnimatorStateMachine rootStateMachine)
        {
            var sitState = sitStateMachine.AddState("Sit");
            sitState.motion = sitAnimation;

            var sittingState = sitStateMachine.AddState("Sitting");
            sitState.motion = sittingAnimation;

            var standState = sitStateMachine.AddState("Stand");
            standState.motion = standAnimation;

            var beginSitTransition = rootStateMachine.defaultState.AddTransition(sitState);
            beginSitTransition.hasExitTime = false;
            beginSitTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Sit.Name);
            beginSitTransition.AddCondition(
                AnimatorConditionMode.Equals, id, AnimatorParameters.SitTypeId.Name);

            var sitToSittingTransition = sitState.AddTransition(sittingState);
            sitToSittingTransition.hasExitTime = true;

            var sitToStandTransition = sitState.AddTransition(standState);
            sitToStandTransition.hasExitTime = false;
            sitToStandTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Stand.Name);

            var sittingToStandTransition = sittingState.AddTransition(standState);
            sittingToStandTransition.hasExitTime = false;
            sittingToStandTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Stand.Name);

            var endStandTransition = standState.AddExitTransition();
            endStandTransition.hasExitTime = true;
        }
    }
}