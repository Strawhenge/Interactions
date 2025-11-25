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

            var id = SitTypeIdHelper.GenerateSitTypeId(sitLayer);

            AddSitType(
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

        static void AddSitType(
            int id,
            string name,
            AnimationClip sitAnimation,
            AnimationClip sittingAnimation,
            AnimationClip standAnimation,
            AnimatorStateMachine sitStateMachine,
            AnimatorStateMachine rootStateMachine)
        {
            var sitTypeStateMachine = sitStateMachine.AddStateMachine(name);
            sitStateMachine.AddStateMachineExitTransition(sitTypeStateMachine);

            var sitState = sitTypeStateMachine.AddState(AnimatorStates.Sit);
            sitState.motion = sitAnimation;

            var sittingState = sitTypeStateMachine.AddState(AnimatorStates.Sitting);
            sittingState.motion = sittingAnimation;

            var standState = sitTypeStateMachine.AddState(AnimatorStates.Stand);
            standState.motion = standAnimation;

            var beginSitTransition = rootStateMachine.defaultState.AddTransition(sitState);
            beginSitTransition.hasExitTime = false;
            beginSitTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Sit.Name);
            beginSitTransition.AddCondition(
                AnimatorConditionMode.Equals, id, AnimatorParameters.SitTypeId.Name);

            var sitToSittingTransition = sitState.AddTransition(sittingState);
            sitToSittingTransition.hasExitTime = true;

            var sitToStandTransition = sitState.AddTransition(standState);
            sitToStandTransition.hasExitTime = false;
            sitToStandTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Stand.Name);

            var sittingToStandTransition = sittingState.AddTransition(standState);
            sittingToStandTransition.hasExitTime = false;
            sittingToStandTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Stand.Name);

            var endStandTransition = standState.AddExitTransition();
            endStandTransition.hasExitTime = true;
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