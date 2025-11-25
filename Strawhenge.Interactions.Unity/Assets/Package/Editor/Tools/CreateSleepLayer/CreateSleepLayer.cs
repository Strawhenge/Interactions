using Strawhenge.Interactions.Unity.Sleep;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using AnimatorParameters = Strawhenge.Interactions.Unity.Sleep.AnimatorParameters;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateSleepLayer
    {
        public static void Create(AnimatorController animatorController)
        {
            AddParameters(animatorController);
            AddLayer(animatorController);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            animatorController.EnsureParametersExist(
                (AnimatorParameters.Sleep, AnimatorControllerParameterType.Trigger),
                (AnimatorParameters.SleepTypeId, AnimatorControllerParameterType.Int),
                (AnimatorParameters.WakeUp, AnimatorControllerParameterType.Trigger));
        }

        static void AddLayer(AnimatorController animatorController)
        {
            var layer = new AnimatorControllerLayer
            {
                name = "Sleep",
                defaultWeight = 1,
                stateMachine = new AnimatorStateMachine()
            };

            animatorController.AddLayer(layer);

            AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(animatorController));

            var rootStateMachine = layer.stateMachine;
            rootStateMachine.AddState("Default");

            var sitStateMachine = rootStateMachine.AddStateMachine("Sit");
            sitStateMachine.AddStateMachineBehaviour<SleepStateMachine>();
        }
    }
}