using Strawhenge.Interactions.Unity.Emotes;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateEmotesLayer
    {
        public static void Create(
            AnimatorController animatorController,
            string layerName,
            AvatarMask avatarMask = null)
        {
            AddParameters(animatorController);
            AddLayer(animatorController, layerName, avatarMask);

            EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssets();
        }

        static void AddParameters(AnimatorController animatorController)
        {
            animatorController.EnsureParametersExist(
                (AnimatorParameters.BeginEmote, AnimatorControllerParameterType.Trigger),
                (AnimatorParameters.EndEmote, AnimatorControllerParameterType.Trigger),
                (AnimatorParameters.EmoteId, AnimatorControllerParameterType.Int));
        }

        static void AddLayer(AnimatorController animatorController, string layerName, AvatarMask avatarMask)
        {
            var layer = animatorController.CreateLayer(layerName, avatarMask);

            var rootStateMachine = layer.stateMachine;
            rootStateMachine.AddState("Default");

            var emotesStateMachine = rootStateMachine.AddStateMachine("Emotes");
            emotesStateMachine.AddStateMachineBehaviour<EmotesStateMachine>();
        }
    }
}