using Strawhenge.Common.Unity;
using Strawhenge.Interactions.Unity.PositionPlacement;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(PositionPlacementScript))]
    public class PositionPlacementScriptEditor : UnityEditor.Editor
    {
        PositionPlacementScript _positionPlacement;
        Transform _position;
        PositionPlacementArgsScriptableObject _args;

        void OnEnable()
        {
            _positionPlacement ??= target as PositionPlacementScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            _position = EditorGUILayout.ObjectField(
                "Position",
                _position,
                typeof(Transform),
                allowSceneObjects: true) as Transform;

            _args = EditorGUILayout.ObjectField(
                "Args",
                _args,
                typeof(PositionPlacementArgsScriptableObject),
                allowSceneObjects: false) as PositionPlacementArgsScriptableObject;

            if (GUILayout.Button(nameof(PositionPlacementController.PlaceAt)) &&
                _position != null &&
                _args != null)
            {
                _positionPlacement.PositionPlacementController.PlaceAt(
                    _position.GetPositionAndRotation(),
                    _args);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}