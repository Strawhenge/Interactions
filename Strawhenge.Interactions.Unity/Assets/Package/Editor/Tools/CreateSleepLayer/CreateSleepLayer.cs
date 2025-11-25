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
            bool hasSleepParameter = false;
            bool hasSleepTypeIdParameter = false;
            bool hasWakeUpParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == AnimatorParameters.Sleep.Name)
                    hasSleepParameter = true;

                if (parameter.name == AnimatorParameters.SleepTypeId.Name)
                    hasSleepTypeIdParameter = true;

                if (parameter.name == AnimatorParameters.WakeUp.Name)
                    hasWakeUpParameter = true;
            }

            if (!hasSleepParameter)
                animatorController
                    .AddParameter(AnimatorParameters.Sleep.Name, AnimatorControllerParameterType.Trigger);

            if (!hasSleepTypeIdParameter)
                animatorController
                    .AddParameter(AnimatorParameters.SleepTypeId.Name, AnimatorControllerParameterType.Int);

            if (!hasWakeUpParameter)
                animatorController
                    .AddParameter(AnimatorParameters.WakeUp.Name, AnimatorControllerParameterType.Trigger);
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