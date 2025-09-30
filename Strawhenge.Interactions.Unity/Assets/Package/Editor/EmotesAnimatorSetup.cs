using Strawhenge.Interactions.Unity.Emotes;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class EmotesAnimatorSetup
    {
        public static void Setup(AnimatorController animatorController)
        {
            AddParameters(animatorController);
            AddSubStateMachines(animatorController);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            bool hasBeginParameter = false;
            bool hasEndParameter = false;
            bool hasLayerIdParameter = false;
            bool hasRepeatingParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == AnimatorParameters.BeginEmote.Name)
                    hasBeginParameter = true;

                if (parameter.name == AnimatorParameters.EndEmote.Name)
                    hasEndParameter = true;

                if (parameter.name == AnimatorParameters.EmoteLayerId.Name)
                    hasLayerIdParameter = true;

                if (parameter.name == AnimatorParameters.RepeatingEmote.Name)
                    hasRepeatingParameter = true;
            }

            if (!hasBeginParameter)
                animatorController
                    .AddParameter(AnimatorParameters.BeginEmote.Name, AnimatorControllerParameterType.Trigger);

            if (!hasEndParameter)
                animatorController
                    .AddParameter(AnimatorParameters.EndEmote.Name, AnimatorControllerParameterType.Trigger);

            if (!hasLayerIdParameter)
                animatorController
                    .AddParameter(AnimatorParameters.EmoteLayerId.Name, AnimatorControllerParameterType.Int);

            if (!hasRepeatingParameter)
                animatorController
                    .AddParameter(AnimatorParameters.RepeatingEmote.Name, AnimatorControllerParameterType.Bool);
        }

        static void AddSubStateMachines(AnimatorController animatorController)
        {
            for (var i = 0; i < animatorController.layers.Length; i++)
            {
                var rootStateMachine = animatorController.layers[i].stateMachine;
                AddSubStateMachine(rootStateMachine, layerId: i);
            }
        }

        static void AddSubStateMachine(AnimatorStateMachine rootStateMachine, int layerId)
        {
            var emotesStateMachine = rootStateMachine.AddStateMachine("Emotes");
            emotesStateMachine.AddStateMachineBehaviour<EmotesStateMachine>();

            var emoteState = emotesStateMachine.AddState("Emote");
            // TODO Set animation clip

            var anyStateTransition = rootStateMachine.AddAnyStateTransition(emoteState);
            anyStateTransition.hasExitTime = false;
            anyStateTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.BeginEmote.Name);
            anyStateTransition
                .AddCondition(AnimatorConditionMode.Equals, layerId, AnimatorParameters.EmoteLayerId.Name);

            var endEmoteTransition = emoteState.AddExitTransition();
            endEmoteTransition.hasExitTime = false;
            endEmoteTransition
                .AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.EndEmote.Name);

            var animationEndedTransition = emoteState.AddExitTransition();
            animationEndedTransition.hasExitTime = true;
            animationEndedTransition
                .AddCondition(AnimatorConditionMode.IfNot, 0, AnimatorParameters.RepeatingEmote.Name);

            if (rootStateMachine.defaultState != null && rootStateMachine.defaultState != emoteState)
                rootStateMachine.AddStateMachineTransition(emotesStateMachine, rootStateMachine.defaultState);
        }
    }
}