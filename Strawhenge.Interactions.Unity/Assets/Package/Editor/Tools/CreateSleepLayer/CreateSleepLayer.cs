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
            var layer = animatorController.CreateLayer("Sleep");

            var rootStateMachine = layer.stateMachine;
            rootStateMachine.AddState("Default");

            var sitStateMachine = rootStateMachine.AddStateMachine("Sleep");
            sitStateMachine.AddStateMachineBehaviour<SleepStateMachine>();
        }
    }
}