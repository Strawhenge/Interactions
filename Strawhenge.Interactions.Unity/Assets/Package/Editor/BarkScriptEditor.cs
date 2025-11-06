using System;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(BarkScript))]
    public class BarkScriptEditor : UnityEditor.Editor
    {
        BarkScript _barkScript;
        BarkScriptableObject _bark;

        void OnEnable()
        {
            _barkScript ??= target as BarkScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            _bark = EditorGUILayout.ObjectField(
                "Bark",
                _bark,
                typeof(BarkScriptableObject),
                false) as BarkScriptableObject;

            EditorGUILayout.BeginHorizontal();
           
            if (GUILayout.Button(nameof(BarkController.Play)) && _bark != null)
                _barkScript.BarkController.Play(_bark);

            if (GUILayout.Button(nameof(BarkController.Stop)))
                _barkScript.BarkController.Stop();
     
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
    }
}