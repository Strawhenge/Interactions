using Strawhenge.Interactions.Unity.Sleep;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(SleepScript))]
    public class SleepScriptEditor : UnityEditor.Editor
    {
        SleepScript _sleep;
        SleepAnimationsScriptableObject _animations;

        void OnEnable()
        {
            _sleep ??= target as SleepScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            _animations = EditorGUILayout.ObjectField(
                "Animations",
                _animations,
                typeof(SleepAnimationsScriptableObject),
                allowSceneObjects: false) as SleepAnimationsScriptableObject;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(SleepController.GoToSleep)))
                _sleep.SleepController.GoToSleep(_animations);

            if (GUILayout.Button(nameof(SleepController.WakeUp)))
                _sleep.SleepController.WakeUp();

            EditorGUILayout.EndHorizontal();
        }
    }
}