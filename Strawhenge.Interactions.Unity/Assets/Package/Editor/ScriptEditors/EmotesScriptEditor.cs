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
            EditorGUILayout.Separator();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            EditorGUILayout.LabelField("Current:", GetCurrentEmoteText());

            _emote = (EmoteScriptableObject)EditorGUILayout
                .ObjectField("Emote", _emote, typeof(EmoteScriptableObject), false);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(nameof(EmoteController.Perform)) && _emote != null)
            {
                var emote = _emote;
                _emotesScript.EmoteController.Perform(
                    _emote,
                    () => Debug.Log($"Emote ended: '{emote.name}'", emote));
            }

            if (GUILayout.Button(nameof(EmoteController.End)))
                _emotesScript.EmoteController.End();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }

        string GetCurrentEmoteText()
        {
            if (!Application.isPlaying)
                return string.Empty;

            return _emotesScript.EmoteController.Current
                .Map(x => x.name)
                .Reduce(() => string.Empty);
        }
    }
}