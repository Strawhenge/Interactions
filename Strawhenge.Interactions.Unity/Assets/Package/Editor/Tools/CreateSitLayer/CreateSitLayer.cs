using Strawhenge.Interactions.Unity.Sit;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using AnimatorParameters = Strawhenge.Interactions.Unity.Sit.AnimatorParameters;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateSitLayer
    {
        public static void Create(
            AnimatorController animatorController,
            AnimationClip defaultSitAnimation,
            AnimationClip defaultSittingAnimation,
            AnimationClip defaultStandAnimation)
        {
            AddParameters(animatorController);
            AddLayer(
                animatorController,
                defaultSitAnimation,
                defaultSittingAnimation,
                defaultStandAnimation);
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

        static void AddLayer(
            AnimatorController animatorController,
            AnimationClip defaultSitAnimation,
            AnimationClip defaultSittingAnimation,
            AnimationClip defaultStandAnimation)
        {
            var layer = new AnimatorControllerLayer
            {
                name = "Sit",
                defaultWeight = 1,
                stateMachine = new AnimatorStateMachine()
            };

            animatorController.AddLayer(layer);

            AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(animatorController));

            var rootStateMachine = layer.stateMachine;
            rootStateMachine.AddState("Default");

            var sitStateMachine = rootStateMachine.AddStateMachine("Sit");
            sitStateMachine.AddStateMachineBehaviour<SitStateMachine>();

            var sitState = sitStateMachine.AddState("Sit");
            sitState.motion = defaultSitAnimation;

            var sittingState = sitStateMachine.AddState("Sitting");
            sitState.motion = defaultSittingAnimation;

            var standState = sitStateMachine.AddState("Stand");
            standState.motion = defaultStandAnimation;

            var beginSitTransition = rootStateMachine.defaultState.AddTransition(sitState);
            beginSitTransition.hasExitTime = false;
            beginSitTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Sit.Name);

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