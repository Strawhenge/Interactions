using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class EmotesAnimatorSetup
    {
        public static void Setup(AnimatorController animatorController)
        {
            AddParameters(animatorController);
        }

        static void AddParameters(AnimatorController animatorController)
        {
            // TODO Move to single location
            const string BeginEmote = "Begin Emote";
            const string EndEmote = "End Emote";

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
    }
}