using Strawhenge.Interactions.Unity.PositionPlacement;
using System;
using UnityEditor;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(PositionPlacementScript))]
    public class PositionPlacementScriptEditor : UnityEditor.Editor
    {
        PositionPlacementScript _positionPlacement;

        void OnEnable()
        {
            _positionPlacement ??= target as PositionPlacementScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
        }
    }
}