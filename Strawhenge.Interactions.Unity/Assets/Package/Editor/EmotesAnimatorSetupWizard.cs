using Strawhenge.Common;
using Strawhenge.Interactions.Unity.Emotes;
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

        readonly Dictionary<string, bool> _enabledLayersByName = new Dictionary<string, bool>();
        readonly Dictionary<string, int> _layerIdsByName = new();
        readonly Dictionary<string, EmoteLayerIdScriptableObject> _layerIdScriptableObjectsByName = new();
        AnimatorController _selectedController;
        string _assetsParentFolder;
        string _assetsFolder;

        void OnWizardCreate()
        {
            if (!AssetDatabase.IsValidFolder(_assetsFolder))
                AssetDatabase.CreateFolder(_assetsParentFolder, _animatorController.name);

            var animationClipPath = $"{_assetsFolder}/{PlaceholderAnimationClip.Name}.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            foreach (var layer in _animatorController.layers)
            {
                var layerId = _layerIdsByName[layer.name];

                if (!_layerIdScriptableObjectsByName.TryGetValue(layer.name, out var scriptableObject))
                {
                    var scriptableObjectPath = $"{_assetsFolder}/{layer.name}.asset";
                    scriptableObject = CreateInstance<EmoteLayerIdScriptableObject>();
                    AssetDatabase.CreateAsset(scriptableObject, scriptableObjectPath);
                }

                scriptableObject.Id = layerId;
            }

            AssetDatabase.SaveAssets();

            var enabledLayerIdsName = new Dictionary<string, int>();
            foreach (var layer in _animatorController.layers)
            {
                if (_enabledLayersByName[layer.name])
                    enabledLayerIdsName[layer.name] = _layerIdsByName[layer.name];
            }

            EmotesAnimatorSetup.Setup(_animatorController, animationClip, enabledLayerIdsName);
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

            _layerIdScriptableObjectsByName.Clear();
            AssetDatabase
                .FindAssets(
                    $"t:{typeof(EmoteLayerIdScriptableObject).Name}",
                    new[] { _assetsFolder })
                .Select(guid =>
                    AssetDatabase.LoadAssetAtPath<EmoteLayerIdScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .ExcludeNull()
                .ForEach(layerIdScriptableObject =>
                {
                    _layerIdScriptableObjectsByName[layerIdScriptableObject.name] = layerIdScriptableObject;
                    _layerIdsByName[layerIdScriptableObject.name] = layerIdScriptableObject.Id;
                });

            _enabledLayersByName.Clear();
            foreach (var layer in _selectedController.layers)
                _enabledLayersByName[layer.name] = _layerIdScriptableObjectsByName.ContainsKey(layer.name);
        }

        protected override bool DrawWizardGUI()
        {
            var result = base.DrawWizardGUI();

            foreach (var layerName in _layerIdsByName.Keys.ToArray())
            {
                EditorGUILayout.BeginHorizontal();

                _enabledLayersByName[layerName] = EditorGUILayout.Toggle(_enabledLayersByName[layerName]);

                EditorGUI.BeginDisabledGroup(!_enabledLayersByName[layerName]);

                _layerIdsByName[layerName] = EditorGUILayout.IntField(layerName, _layerIdsByName[layerName]);

                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }

            return result;
        }
    }
}