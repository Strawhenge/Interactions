using Strawhenge.Common.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

        public static AnimatorControllerLayer CreateLayer(
            this AnimatorController animatorController,
            string layerName,
            AvatarMask avatarMask = null)
        {
            var layer = new AnimatorControllerLayer
            {
                name = layerName,
                defaultWeight = 1,
                avatarMask = avatarMask,
                stateMachine = new AnimatorStateMachine()
            };

            animatorController.AddLayer(layer);

            AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(animatorController));

            return layer;
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