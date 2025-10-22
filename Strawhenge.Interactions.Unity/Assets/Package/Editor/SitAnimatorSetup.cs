using Strawhenge.Interactions.Unity.Sit;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class SitAnimatorSetup
    {
        public static void Setup(
            AnimatorController animatorController,
            int layerIndex,
            AnimationClip sitAnimationClip,
            AnimationClip sittingAnimationClip,
            AnimationClip standAnimationClip)
        {
            AddParameters(animatorController);
            AddSubStateMachine(
                animatorController,
                layerIndex,
                sitAnimationClip,
                sittingAnimationClip,
                standAnimationClip);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            bool hasSitParameter = false;
            bool hasStandParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == AnimatorParameters.Sit.Name)
                    hasSitParameter = true;

                if (parameter.name == AnimatorParameters.Stand.Name)
                    hasStandParameter = true;
            }

            if (!hasSitParameter)
                animatorController
                    .AddParameter(AnimatorParameters.Sit.Name, AnimatorControllerParameterType.Trigger);

            if (!hasStandParameter)
                animatorController
                    .AddParameter(AnimatorParameters.Stand.Name, AnimatorControllerParameterType.Trigger);
        }

        static void AddSubStateMachine(
            AnimatorController animatorController,
            int layerIndex,
            AnimationClip sitAnimationClip,
            AnimationClip sittingAnimationClip,
            AnimationClip standAnimationClip)
        {
            var layer = animatorController.layers[layerIndex];
            var rootStateMachine = layer.stateMachine;

            if (ContainsSitSubState(rootStateMachine))
            {
                Debug.Log($"Animator controller already contains Sit sub state.");
                return;
            }

            var sitStateMachine = layer.stateMachine.AddStateMachine("Sit");
            sitStateMachine.AddStateMachineBehaviour<SitStateMachine>();

            var sitState = sitStateMachine.AddState(AnimatorStates.Sit);
            sitState.motion = sitAnimationClip;

            var sittingState = sitStateMachine.AddState(AnimatorStates.Sitting);
            sittingState.motion = sittingAnimationClip;

            var standState = sitStateMachine.AddState(AnimatorStates.Stand);
            standState.motion = standAnimationClip;

            var anyStateToSitTransition = rootStateMachine.AddAnyStateTransition(sitState);
            anyStateToSitTransition.hasExitTime = false;
            anyStateToSitTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.Sit.Name);

            var sitToSittingTransition = sitState.AddTransition(sittingState);
            sitToSittingTransition.hasExitTime = true;

            var sitToStandTransition = sitState.AddTransition(standState);
            sitToStandTransition.hasExitTime = false;
            sitToStandTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.Stand.Name);

            var sittingToStandTransition = sittingState.AddTransition(standState);
            sittingToStandTransition.hasExitTime = false;
            sittingToStandTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.Stand.Name);

            var standToExitTransition = standState.AddExitTransition();
            standToExitTransition.hasExitTime = true;

            rootStateMachine.AddStateMachineTransition(sitStateMachine, rootStateMachine.defaultState);
        }

        static bool ContainsSitSubState(AnimatorStateMachine stateMachine) =>
            stateMachine.stateMachines.Any(subState =>
                subState.stateMachine.behaviours.OfType<SitStateMachine>().Any());
    }
}