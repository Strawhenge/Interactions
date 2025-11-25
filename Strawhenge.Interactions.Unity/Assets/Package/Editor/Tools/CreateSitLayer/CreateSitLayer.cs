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
            bool hasSitTypeIdParameter = false;
            bool hasStandParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == AnimatorParameters.Sit.Name)
                    hasSitParameter = true;

                if (parameter.name == AnimatorParameters.SitTypeId.Name)
                    hasSitTypeIdParameter = true;

                if (parameter.name == AnimatorParameters.Stand.Name)
                    hasStandParameter = true;
            }

            if (!hasSitParameter)
                animatorController
                    .AddParameter(AnimatorParameters.Sit.Name, AnimatorControllerParameterType.Trigger);

            if (!hasSitTypeIdParameter)
                animatorController
                    .AddParameter(AnimatorParameters.SitTypeId.Name, AnimatorControllerParameterType.Int);

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

            SitTypeHelper.AddSitType(
                id: 0,
                name: "Default",
                defaultSitAnimation,
                defaultSittingAnimation,
                defaultStandAnimation,
                sitStateMachine,
                rootStateMachine);
        }
    }
}