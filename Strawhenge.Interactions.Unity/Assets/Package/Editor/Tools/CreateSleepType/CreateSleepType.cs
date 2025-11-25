using Strawhenge.Interactions.Unity.Sleep;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using AnimatorParameters = Strawhenge.Interactions.Unity.Sleep.AnimatorParameters;
using AnimatorStates = Strawhenge.Interactions.Unity.Sleep.AnimatorStates;

namespace Strawhenge.Interactions.Unity.Editor
{
    static class CreateSleepType
    {
        public static void Create(
            AnimatorController animatorController,
            string name,
            AnimationClip layDownAnimation,
            AnimationClip sleepingAnimation,
            AnimationClip wakeUpAnimation)
        {
            AnimatorControllerLayer sleepLayer = null;
            AnimatorStateMachine sleepStateMachine = null;

            foreach (var layer in animatorController.layers)
            foreach (var stateMachine in layer.stateMachine.stateMachines.Select(x => x.stateMachine))
            {
                if (stateMachine.behaviours.OfType<SleepStateMachine>().Any())
                {
                    sleepLayer = layer;
                    sleepStateMachine = stateMachine;
                }
            }

            if (sleepLayer == null || sleepStateMachine == null)
            {
                Debug.LogError("Sleep layer not found.", animatorController);
                return;
            }

            var id = SitTypeIdHelper.GenerateSitTypeId(sleepLayer);

            AddSitType(
                id,
                name,
                layDownAnimation,
                sleepingAnimation,
                wakeUpAnimation,
                sleepStateMachine,
                sleepLayer.stateMachine
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

            var sitState = sitTypeStateMachine.AddState(AnimatorStates.LayDown);
            sitState.motion = sitAnimation;

            var sittingState = sitTypeStateMachine.AddState(AnimatorStates.Sleeping);
            sittingState.motion = sittingAnimation;

            var standState = sitTypeStateMachine.AddState(AnimatorStates.WakeUp);
            standState.motion = standAnimation;

            var beginSitTransition = rootStateMachine.defaultState.AddTransition(sitState);
            beginSitTransition.hasExitTime = false;
            beginSitTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.Sleep.Name);
            beginSitTransition.AddCondition(
                AnimatorConditionMode.Equals, id, AnimatorParameters.SleepTypeId.Name);

            var sitToSittingTransition = sitState.AddTransition(sittingState);
            sitToSittingTransition.hasExitTime = true;

            var sitToStandTransition = sitState.AddTransition(standState);
            sitToStandTransition.hasExitTime = false;
            sitToStandTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.WakeUp.Name);

            var sittingToStandTransition = sittingState.AddTransition(standState);
            sittingToStandTransition.hasExitTime = false;
            sittingToStandTransition.AddCondition(
                AnimatorConditionMode.If, 0, AnimatorParameters.WakeUp.Name);

            var endStandTransition = standState.AddExitTransition();
            endStandTransition.hasExitTime = true;
        }

        static void CreateScriptableObject(int id, string name)
        {
            var scriptableObject = ScriptableObject.CreateInstance<SleepTypeScriptableObject>();

            var serializedObject = new SerializedObject(scriptableObject);
            serializedObject.FindProperty(SleepTypeScriptableObject.IdFieldName).intValue = id;
            serializedObject.ApplyModifiedProperties();

            var directoryPath = SelectionHelper.GetAssetDirectoryPath();
            var scriptableObjectPath = $"{directoryPath}/{name}.asset";
            AssetDatabase.CreateAsset(scriptableObject, scriptableObjectPath);
        }
    }
}