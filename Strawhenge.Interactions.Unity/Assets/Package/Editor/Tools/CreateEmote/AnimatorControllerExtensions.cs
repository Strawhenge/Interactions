using Strawhenge.Interactions.Unity.Emotes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class AnimatorControllerExtensions
    {
        public static IReadOnlyList<AnimatorControllerLayer> GetEmoteLayers(this AnimatorController animatorController)
        {
            return animatorController.layers
                .Where(HasEmoteStateMachine)
                .ToArray();

            static bool HasEmoteStateMachine(AnimatorControllerLayer animatorControllerLayer) =>
                animatorControllerLayer.stateMachine.stateMachines
                    .Any(stateMachine => stateMachine.stateMachine.behaviours.OfType<EmotesStateMachine>().Any());
        }

        public static int GenerateEmoteId(this AnimatorController animatorController)
        {
            var emoteLayers = animatorController.GetEmoteLayers();

            int highestId = 0;
            foreach (var layer in emoteLayers)
            {
                highestId = layer.stateMachine.defaultState.transitions
                    .SelectMany(x => x.conditions
                        .Where(y => y.parameter == AnimatorParameters.EmoteId.Name)
                        .Select(y => (int)y.threshold))
                    .Prepend(highestId)
                    .Max();
            }

            return highestId + 1;
        }
    }
}