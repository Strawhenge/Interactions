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

        readonly Dictionary<string, bool> _enabledLayersByName = new();
        readonly Dictionary<string, int> _layerIdsByName = new();
        readonly Dictionary<string, EmoteLayerIdScriptableObject> _layerIdScriptableObjectsByName = new();
        AnimatorController _selectedController;
        string _assetsParentFolder;
        string _assetsFolder;

        void OnWizardCreate()
        {
            if (_selectedController == null)
            {
                Debug.LogWarning("No controller selected.");
                return;
            }

            EnsureAssetsFolderExists();
            var animationClip = GetOrCreatePlaceholderAnimationClip();
            UpdateScriptableObjects();
            var enabledLayerIdsByName = GenerateEnabledLayerIdsByName();

            EmotesAnimatorSetup.Setup(_animatorController, animationClip, enabledLayerIdsByName);
        }

        Dictionary<string, int> GenerateEnabledLayerIdsByName()
        {
            var enabledLayerIdsByName = new Dictionary<string, int>();
            foreach (var layer in _animatorController.layers)
            {
                if (_enabledLayersByName[layer.name])
                    enabledLayerIdsByName[layer.name] = _layerIdsByName[layer.name];
            }

            return enabledLayerIdsByName;
        }

        void UpdateScriptableObjects()
        {
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
        }

        AnimationClip GetOrCreatePlaceholderAnimationClip()
        {
            var animationClipPath = $"{_assetsFolder}/{PlaceholderAnimationClip.Name}.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            return animationClip;
        }

        void EnsureAssetsFolderExists()
        {
            if (!AssetDatabase.IsValidFolder(_assetsFolder))
                AssetDatabase.CreateFolder(_assetsParentFolder, _animatorController.name);
        }

        void OnWizardUpdate()
        {
            if (_selectedController == _animatorController) return;
            _selectedController = _animatorController;

            if (_selectedController == null) return;

            UpdateAssetsFolder();
            LoadScriptableObjects();
            UpdateEnabledLayers();
            UpdateLayerIds();
        }

        void UpdateEnabledLayers()
        {
            _enabledLayersByName.Clear();
            foreach (var layer in _selectedController.layers)
                _enabledLayersByName[layer.name] = _layerIdScriptableObjectsByName.ContainsKey(layer.name);
        }

        void UpdateLayerIds()
        {
            _layerIdsByName.Clear();

            for (var i = 0; i < _selectedController.layers.Length; i++)
            {
                var layerName = _selectedController.layers[i].name;
                if (_layerIdScriptableObjectsByName.TryGetValue(layerName, out var scriptableObject))
                    _layerIdsByName[layerName] = scriptableObject.Id;
                else
                    _layerIdsByName[layerName] = i;
            }
        }

        void LoadScriptableObjects()
        {
            _layerIdScriptableObjectsByName.Clear();

            if (!AssetDatabase.IsValidFolder(_assetsFolder))
                return;

            // ReSharper disable once UseNameOfInsteadOfTypeOf
            // Justification: `nameof` does not work.
            var assetGuids = AssetDatabase.FindAssets(
                $"t:{typeof(EmoteLayerIdScriptableObject).Name}",
                new[] { _assetsFolder });

            foreach (var assetGuid in assetGuids)
            {
                var layerIdScriptableObject = AssetDatabase
                    .LoadAssetAtPath<EmoteLayerIdScriptableObject>(AssetDatabase.GUIDToAssetPath(assetGuid));

                if (layerIdScriptableObject == null) continue;

                _layerIdScriptableObjectsByName[layerIdScriptableObject.name] = layerIdScriptableObject;
            }
        }

        void UpdateAssetsFolder()
        {
            var path = AssetDatabase.GetAssetPath(_animatorController);
            _assetsParentFolder = path[..path.LastIndexOf('/')];
            _assetsFolder = _assetsParentFolder + "/" + _animatorController.name;
        }

        protected override bool DrawWizardGUI()
        {
            if (_selectedController == null)
                return base.DrawWizardGUI();

            var result = base.DrawWizardGUI();

            foreach (var layer in _selectedController.layers)
            {
                var layerName = layer.name;

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