using Strawhenge.Interactions.Unity.Sleep;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(SleepScript))]
    public class SleepScriptEditor : UnityEditor.Editor
    {
        SleepScript _sleep;
        SleepTypeScriptableObject _sleepType;

        void OnEnable()
        {
            _sleep ??= target as SleepScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            _sleepType = EditorGUILayout.ObjectField(
                "Sleep Type",
                _sleepType,
                typeof(SleepTypeScriptableObject),
                allowSceneObjects: false) as SleepTypeScriptableObject;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(SleepController.GoToSleep)))
                _sleep.SleepController.GoToSleep(_sleepType);

            if (GUILayout.Button(nameof(SleepController.WakeUp)))
                _sleep.SleepController.WakeUp();

            EditorGUILayout.EndHorizontal();
        }
    }
}