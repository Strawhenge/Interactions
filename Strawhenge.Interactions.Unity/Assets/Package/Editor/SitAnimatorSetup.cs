using Strawhenge.Interactions.Unity.Sit;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class SitAnimatorSetup
    {
        public static void Setup(
            AnimatorController animatorController,
            int layerIndex)
        {
            AddParameters(animatorController);
            AddSubStateMachine(animatorController, layerIndex);
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

        static void AddSubStateMachine(AnimatorController animatorController, int layerIndex)
        {
            var layer = animatorController.layers[layerIndex];
            var rootStateMachine = layer.stateMachine;

            var sitStateMachine = layer.stateMachine.AddStateMachine("Sit");

            var sitState = sitStateMachine.AddState("Sit");
            var sittingState = sitStateMachine.AddState("Sitting");
            var standState = sitStateMachine.AddState("Stand");

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
    }
}