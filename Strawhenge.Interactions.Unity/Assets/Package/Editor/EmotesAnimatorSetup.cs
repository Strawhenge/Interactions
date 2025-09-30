using Strawhenge.Interactions.Unity.Emotes;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class EmotesAnimatorSetup
    {
        // TODO Move to single location
        const string BeginEmote = "Begin Emote";
        const string EndEmote = "End Emote";

        public static void Setup(AnimatorController animatorController)
        {
            AddParameters(animatorController);
            AddSubStateMachine(animatorController);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            bool hasBeginParameter = false;
            bool hasEndParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == BeginEmote)
                    hasBeginParameter = true;

                if (parameter.name == EndEmote)
                    hasEndParameter = true;
            }

            if (!hasBeginParameter)
                animatorController.AddParameter(BeginEmote, AnimatorControllerParameterType.Trigger);

            if (!hasEndParameter)
                animatorController.AddParameter(EndEmote, AnimatorControllerParameterType.Trigger);
        }

        static void AddSubStateMachine(AnimatorController animatorController)
        {
            AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;

            var emotesStateMachine = rootStateMachine.AddStateMachine("Emotes");
            emotesStateMachine.AddStateMachineBehaviour<EmotesStateMachine>();

            var emoteState = emotesStateMachine.AddState("Emote");
            // TODO Set animation clip

            var anyStateTransition = rootStateMachine.AddAnyStateTransition(emoteState);
            anyStateTransition.AddCondition(AnimatorConditionMode.If, 0, BeginEmote);
            anyStateTransition.hasExitTime = false;

            var endEmoteTransition = emoteState.AddExitTransition();
            endEmoteTransition.AddCondition(AnimatorConditionMode.If, 0, EndEmote);
            endEmoteTransition.hasExitTime = false;

            var animationEndedTransition = emoteState.AddExitTransition();
            animationEndedTransition.hasExitTime = true;

            if (rootStateMachine.defaultState != null && rootStateMachine.defaultState != emoteState)
                rootStateMachine.AddStateMachineTransition(emotesStateMachine, rootStateMachine.defaultState);
        }
    }
}