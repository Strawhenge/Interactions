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

            if (GUILayout.Button(nameof(EmoteController.Perform)) && _emote != null)
                _emotesScript.EmoteController
                    .Perform(_emote, () => Debug.Log($"Emote ended: '{_emote.name}'", _emote));

            if (GUILayout.Button(nameof(EmoteController.End)))
                _emotesScript.EmoteController.End();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
    }
}