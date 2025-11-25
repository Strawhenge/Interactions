using Strawhenge.Interactions.Unity.Emotes;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class AnimatorControllerExtensions
    {
        public static IReadOnlyList<AnimatorControllerLayer> GetLayersContaining<TBehaviour>(
            this AnimatorController animatorController)
            where TBehaviour : StateMachineBehaviour
        {
            return animatorController.layers
                .Where(HasEmoteStateMachine)
                .ToArray();

            static bool HasEmoteStateMachine(AnimatorControllerLayer animatorControllerLayer) =>
                animatorControllerLayer.stateMachine.stateMachines
                    .Any(stateMachine => stateMachine.stateMachine.behaviours.OfType<TBehaviour>().Any());
        }
    }
}