using Strawhenge.Interactions.Unity.Sit;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(SitScript))]
    public class SitScriptEditor : UnityEditor.Editor
    {
        SitScript _sit;
        SitTypeScriptableObject _sitType;

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

            _sitType = EditorGUILayout.ObjectField(
                "Sit Type",
                _sitType,
                typeof(SitAnimationsScriptableObject),
                allowSceneObjects: false) as SitTypeScriptableObject;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(SitController.Sit)))
                _sit.SitController.Sit(_sitType);

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