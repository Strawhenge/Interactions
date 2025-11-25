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
            bool hasBeginParameter = false;
            bool hasEndParameter = false;
            bool hasEmoteIdParameter = false;

            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.name == AnimatorParameters.BeginEmote.Name)
                    hasBeginParameter = true;

                if (parameter.name == AnimatorParameters.EndEmote.Name)
                    hasEndParameter = true;

                if (parameter.name == AnimatorParameters.EmoteId.Name)
                    hasEmoteIdParameter = true;
            }

            if (!hasBeginParameter)
                animatorController
                    .AddParameter(AnimatorParameters.BeginEmote.Name, AnimatorControllerParameterType.Trigger);

            if (!hasEndParameter)
                animatorController
                    .AddParameter(AnimatorParameters.EndEmote.Name, AnimatorControllerParameterType.Trigger);

            if (!hasEmoteIdParameter)
                animatorController
                    .AddParameter(AnimatorParameters.EmoteId.Name, AnimatorControllerParameterType.Int);
        }

        static void AddLayer(AnimatorController animatorController, string layerName, AvatarMask avatarMask)
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

            var rootStateMachine = layer.stateMachine;
            rootStateMachine.AddState("Default");

            var emotesStateMachine = rootStateMachine.AddStateMachine("Emotes");
            emotesStateMachine.AddStateMachineBehaviour<EmotesStateMachine>();
        }
    }
}