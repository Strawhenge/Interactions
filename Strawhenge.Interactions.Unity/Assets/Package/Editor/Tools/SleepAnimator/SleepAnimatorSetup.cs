using Strawhenge.Interactions.Unity.Sleep;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class SleepAnimatorSetup
    {
        public static void Setup(
            AnimatorController animatorController,
            int layerIndex,
            AnimationClip sleepAnimationClip,
            AnimationClip sleeptingAnimationClip,
            AnimationClip standAnimationClip)
        {
            AddParameters(animatorController);
            AddSubStateMachine(
                animatorController,
                layerIndex,
                sleepAnimationClip,
                sleeptingAnimationClip,
                standAnimationClip);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            bool hasSleepParameter = false;
            bool hasStandParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == AnimatorParameters.Sleep.Name)
                    hasSleepParameter = true;

                if (parameter.name == AnimatorParameters.WakeUp.Name)
                    hasStandParameter = true;
            }

            if (!hasSleepParameter)
                animatorController
                    .AddParameter(AnimatorParameters.Sleep.Name, AnimatorControllerParameterType.Trigger);

            if (!hasStandParameter)
                animatorController
                    .AddParameter(AnimatorParameters.WakeUp.Name, AnimatorControllerParameterType.Trigger);
        }

        static void AddSubStateMachine(
            AnimatorController animatorController,
            int layerIndex,
            AnimationClip sleepAnimationClip,
            AnimationClip sleeptingAnimationClip,
            AnimationClip standAnimationClip)
        {
            var layer = animatorController.layers[layerIndex];
            var rootStateMachine = layer.stateMachine;

            if (ContainsSleepSubState(rootStateMachine))
            {
                Debug.Log($"Animator controller already contains Sleep sub state.");
                return;
            }

            var sleepStateMachine = layer.stateMachine.AddStateMachine("Sleep");
            sleepStateMachine.AddStateMachineBehaviour<SleepStateMachine>();

            var layDownState = sleepStateMachine.AddState(AnimatorStates.LayDown);
            layDownState.motion = sleepAnimationClip;

            var sleepingState = sleepStateMachine.AddState(AnimatorStates.Sleeping);
            sleepingState.motion = sleeptingAnimationClip;

            var getUpState = sleepStateMachine.AddState(AnimatorStates.GetUp);
            getUpState.motion = standAnimationClip;

            var anyStateToSleepTransition = rootStateMachine.AddAnyStateTransition(layDownState);
            anyStateToSleepTransition.hasExitTime = false;
            anyStateToSleepTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.Sleep.Name);

            var layDownToSleepingTransition = layDownState.AddTransition(sleepingState);
            layDownToSleepingTransition.hasExitTime = true;

            var layDownToGetUpTransition = layDownState.AddTransition(getUpState);
            layDownToGetUpTransition.hasExitTime = false;
            layDownToGetUpTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.WakeUp.Name);

            var sleepingToGetUpTransition = sleepingState.AddTransition(getUpState);
            sleepingToGetUpTransition.hasExitTime = false;
            sleepingToGetUpTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.WakeUp.Name);

            var standToExitTransition = getUpState.AddExitTransition();
            standToExitTransition.hasExitTime = true;

            rootStateMachine.AddStateMachineTransition(sleepStateMachine, rootStateMachine.defaultState);
        }

        static bool ContainsSleepSubState(AnimatorStateMachine stateMachine) =>
            stateMachine.stateMachines.Any(subState =>
                subState.stateMachine.behaviours.OfType<SleepStateMachine>().Any());
    }
}