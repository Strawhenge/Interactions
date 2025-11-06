using Strawhenge.Interactions.Furniture;
using Strawhenge.Interactions.Unity.Furniture;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(FurnitureUserScript))]
    public class FurnitureUserScriptEditor : UnityEditor.Editor
    {
        FurnitureUserScript _furnitureUser;
        FurnitureScript _furniture;

        void OnEnable()
        {
            _furnitureUser ??= target as FurnitureUserScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            _furniture = EditorGUILayout.ObjectField(
                "Furniture",
                _furniture,
                typeof(FurnitureScript),
                allowSceneObjects: true) as FurnitureScript;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(FurnitureUser<UserContext>.Use)) && _furniture != null)
                _furnitureUser.User.Use(_furniture.Furniture);

            if (GUILayout.Button(nameof(FurnitureUser<UserContext>.EndUse)))
                _furnitureUser.User.EndUse();

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }
    }
}