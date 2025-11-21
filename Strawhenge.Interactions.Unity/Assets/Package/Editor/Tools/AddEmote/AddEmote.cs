using Strawhenge.Interactions.Unity.Emotes;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class AddEmote
    {
        public static void Add(AddEmoteArgs args)
        {
            var emoteId = args.AnimatorController.GenerateEmoteId();

            var layer = args.AnimatorController.layers
                .FirstOrDefault(x => x.name == args.LayerName);

            if (layer == null)
            {
                Debug.LogError($"Layer '{args.LayerName}' not found.", args.AnimatorController);
                return;
            }

            var rootStateMachine = layer.stateMachine;
            var emoteStateMachine = layer.stateMachine.stateMachines
                .Select(x => x.stateMachine)
                .FirstOrDefault(x => x.behaviours.OfType<EmotesStateMachine>().Any());

            if (emoteStateMachine == null)
            {
                Debug.LogError(
                    $"Layer '{args.LayerName}' does not contain '{nameof(EmotesStateMachine)}'.",
                    args.AnimatorController);
                return;
            }

            var emoteState = emoteStateMachine.AddState(args.EmoteName);
            emoteState.motion = args.Animation;

            var beginEmoteTransition = rootStateMachine.defaultState.AddTransition(emoteState);
            beginEmoteTransition.AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.BeginEmote.Name);
            beginEmoteTransition.AddCondition(AnimatorConditionMode.Equals, emoteId, AnimatorParameters.EmoteId.Name);
            beginEmoteTransition.hasExitTime = false;

            var endEmoteTransition = emoteState.AddExitTransition();
            endEmoteTransition.AddCondition(AnimatorConditionMode.If, 0, AnimatorParameters.EndEmote.Name);
            endEmoteTransition.hasExitTime = false;

            if (!args.IsRepeating)
            {
                var animationEndedTransition = emoteState.AddExitTransition();
                animationEndedTransition.hasExitTime = true;
            }

            EditorUtility.SetDirty(args.AnimatorController);
            AssetDatabase.SaveAssets();

            CreateScriptableObject(emoteId, args);
        }

        static void CreateScriptableObject(int emoteId, AddEmoteArgs args)
        {
            var scriptableObject = ScriptableObject.CreateInstance<EmoteScriptableObject>();
            var serializedObject = new SerializedObject(scriptableObject);
            serializedObject.FindProperty(EmoteScriptableObject.IdFieldName).intValue = emoteId;
            serializedObject.FindProperty(EmoteScriptableObject.UseRootMotionFieldName).boolValue = args.UseRootMotion;
            serializedObject.ApplyModifiedProperties();

            var directoryPath = SelectionHelper.GetAssetDirectoryPath();
            var scriptableObjectPath = $"{directoryPath}/{args.EmoteName}.asset";
            AssetDatabase.CreateAsset(scriptableObject, scriptableObjectPath);
        }
    }
}