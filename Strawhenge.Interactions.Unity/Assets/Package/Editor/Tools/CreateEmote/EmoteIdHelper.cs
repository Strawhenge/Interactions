using Strawhenge.Interactions.Unity.Emotes;
using System.Linq;
using UnityEditor.Animations;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class EmoteIdHelper
    {
        public static int GenerateEmoteId(AnimatorController animatorController)
        {
            var emoteLayers = animatorController.GetLayersContaining<EmotesStateMachine>();

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