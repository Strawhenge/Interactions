using Strawhenge.Interactions.Unity.Emotes;
using UnityEditor;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    [CustomEditor(typeof(EmotesScript))]
    public class EmotesScriptEditor : UnityEditor.Editor
    {
        EmotesScript _emotesScript;
        EmoteScriptableObject _emote;

        void OnEnable()
        {
            _emotesScript ??= target as EmotesScript;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            _emote = (EmoteScriptableObject)EditorGUILayout
                .ObjectField("Emote", _emote, typeof(EmoteScriptableObject), false);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(EmotesScript.Perform)) && _emote != null)
                _emotesScript.Perform(_emote);

            if (GUILayout.Button(nameof(EmotesScript.End)))
                _emotesScript.End();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
    }
}