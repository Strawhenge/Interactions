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
        string _assetsParentFolder;
        string _assetsFolder;

        void OnWizardCreate()
        {
            if (!AssetDatabase.IsValidFolder(_assetsFolder))
                AssetDatabase.CreateFolder(_assetsParentFolder, _animatorController.name);

            var animationClipPath = $"{_assetsFolder}/Emote.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            EmotesAnimatorSetup.Setup(_animatorController, animationClip, _layerIdsByName);
        }

        void OnWizardUpdate()
        {
            if (_selectedController == _animatorController) return;
            _selectedController = _animatorController;

            if (_selectedController == null) return;

            var path = AssetDatabase.GetAssetPath(_animatorController);
            _assetsParentFolder = path[..path.LastIndexOf('/')];
            _assetsFolder = _assetsParentFolder + "/" + _animatorController.name;

            _layerIdsByName.Clear();
            for (var i = 0; i < _selectedController.layers.Length; i++)
                _layerIdsByName[_selectedController.layers[i].name] = i;
        }

        protected override bool DrawWizardGUI()
        {
            var result = base.DrawWizardGUI();

            foreach (var layerName in _layerIdsByName.Keys.ToArray())
                _layerIdsByName[layerName] = EditorGUILayout.IntField(layerName, _layerIdsByName[layerName]);

            return result;
        }
    }
}