using Strawhenge.Interactions.Unity.Sit;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateSitType
    {
        public static void Create(
            AnimatorController animatorController,
            string name,
            AnimationClip sitAnimation,
            AnimationClip sittingAnimation,
            AnimationClip standAnimation)
        {
            AnimatorControllerLayer sitLayer = null;
            AnimatorStateMachine sitStateMachine = null;

            foreach (var layer in animatorController.layers)
            foreach (var stateMachine in layer.stateMachine.stateMachines.Select(x => x.stateMachine))
            {
                if (stateMachine.behaviours.OfType<SitStateMachine>().Any())
                {
                    sitLayer = layer;
                    sitStateMachine = stateMachine;
                }
            }

            if (sitLayer == null || sitStateMachine == null)
            {
                Debug.LogError("Sit layer not found.", animatorController);
                return;
            }

            var id = sitLayer.GenerateSitTypeId();

            SitTypeHelper.AddSitType(
                id,
                name,
                sitAnimation,
                sittingAnimation,
                standAnimation,
                sitStateMachine,
                sitLayer.stateMachine
            );

            EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssets();

            CreateScriptableObject(id, name);
        }

        static void CreateScriptableObject(int id, string name)
        {
            var scriptableObject = ScriptableObject.CreateInstance<SitTypeScriptableObject>();

            var serializedObject = new SerializedObject(scriptableObject);
            serializedObject.FindProperty(SitTypeScriptableObject.IdFieldName).intValue = id;
            serializedObject.ApplyModifiedProperties();

            var directoryPath = SelectionHelper.GetAssetDirectoryPath();
            var scriptableObjectPath = $"{directoryPath}/{name}.asset";
            AssetDatabase.CreateAsset(scriptableObject, scriptableObjectPath);
        }
    }
}