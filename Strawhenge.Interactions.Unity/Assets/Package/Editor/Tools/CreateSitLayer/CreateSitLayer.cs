using Strawhenge.Interactions.Unity.Sit;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using AnimatorParameters = Strawhenge.Interactions.Unity.Sit.AnimatorParameters;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateSitLayer
    {
        public static void Create(AnimatorController animatorController)
        {
            AddParameters(animatorController);
            AddLayer(animatorController);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            animatorController.EnsureParametersExist(
                (AnimatorParameters.Sit, AnimatorControllerParameterType.Trigger),
                (AnimatorParameters.SitTypeId, AnimatorControllerParameterType.Int),
                (AnimatorParameters.Stand, AnimatorControllerParameterType.Trigger));
        }

        static void AddLayer(AnimatorController animatorController)
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
        }
    }
}