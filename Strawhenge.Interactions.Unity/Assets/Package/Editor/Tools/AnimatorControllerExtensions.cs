using Strawhenge.Common.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class AnimatorControllerExtensions
    {
        public static void EnsureParametersExist(
            this AnimatorController animatorController,
            params (AnimatorParameter, AnimatorControllerParameterType)[] parameters)
        {
            foreach (var (parameter, parameterType) in parameters)
            {
                if (animatorController.parameters.All(p => p.name != parameter.Name))
                    animatorController.AddParameter(parameter.Name, parameterType);
            }
        }

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