using Strawhenge.Interactions.Unity.Sit;
using System;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(SitScript))]
    public class SitScriptEditor : UnityEditor.Editor
    {
        SitScript _sit;
        SitAnimationsScriptableObject _animations;

        void OnEnable()
        {
            _sit ??= target as SitScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            EditorGUILayout.LabelField($"{nameof(SitController.IsSitting)}: {GetIsSittingText()}");

            _animations = EditorGUILayout.ObjectField(
                "Animations",
                _animations,
                typeof(SitAnimationsScriptableObject),
                allowSceneObjects: false) as SitAnimationsScriptableObject;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(SitController.Sit)))
                _sit.SitController.Sit(_animations);

            if (GUILayout.Button(nameof(SitController.Stand)))
                _sit.SitController.Stand();

            EditorGUILayout.EndHorizontal();
        }

        string GetIsSittingText()
        {
            return Application.isPlaying
                ? _sit.SitController.IsSitting.ToString()
                : "N/A";
        }
    }
}