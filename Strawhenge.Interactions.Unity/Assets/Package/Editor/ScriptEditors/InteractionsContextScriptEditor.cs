using System;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(InteractionsContextScript))]
    public class InteractionsContextScriptEditor : UnityEditor.Editor
    {
        InteractionsContextScript _interactionsContext;

        void OnEnable()
        {
            _interactionsContext ??= target as InteractionsContextScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            var isValid = !Application.isPlaying || _interactionsContext.Context.IsValid;

            isValid = EditorGUILayout.Toggle(
                "Is Valid",
                isValid);

            if (Application.isPlaying)
                _interactionsContext.Context.IsValid = isValid;

            if (GUILayout.Button(nameof(InteractionsContext.Interrupt)))
                _interactionsContext.Context.Interrupt();

            EditorGUI.EndDisabledGroup();
        }
    }
}