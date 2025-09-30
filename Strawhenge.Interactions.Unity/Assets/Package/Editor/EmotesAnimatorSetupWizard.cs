using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class EmotesAnimatorSetupWizard : ScriptableWizard
    {
        const string Name = "Emotes Animator Setup";

        [MenuItem("Strawhenge/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<EmotesAnimatorSetupWizard>(Name, "Set Up");
        }

        [SerializeField] AnimatorController _animatorController;

        readonly Dictionary<string, int> _layerIdsByName = new();
        AnimatorController _selectedController;

        void OnWizardCreate()
        {
            var path = AssetDatabase.GetAssetPath(_animatorController);
            var parentFolder = path[..path.LastIndexOf('/')];
            var folder = parentFolder + "/" + _animatorController.name;

            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder(parentFolder, _animatorController.name);

            var animationClipPath = $"{folder}/Emote.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            EmotesAnimatorSetup.Setup(_animatorController, animationClip, _layerIdsByName);
        }

        protected override bool DrawWizardGUI()
        {
            if (_selectedController != _animatorController)
            {
                _selectedController = _animatorController;

                _layerIdsByName.Clear();
                for (int i = 0; i < _selectedController.layers.Length; i++)
                    _layerIdsByName[_selectedController.layers[i].name] = i;
            }

            var result = base.DrawWizardGUI();

            foreach (var layerName in _layerIdsByName.Keys.ToArray())
                _layerIdsByName[layerName] = EditorGUILayout.IntField(layerName, _layerIdsByName[layerName]);

            return result;
        }
    }
}