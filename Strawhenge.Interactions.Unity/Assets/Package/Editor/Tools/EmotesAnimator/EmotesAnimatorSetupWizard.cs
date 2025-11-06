using Strawhenge.Interactions.Unity.Emotes;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class EmotesAnimatorSetupWizard : ScriptableWizard
    {
        const string Name = "Emotes Animator Setup";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<EmotesAnimatorSetupWizard>(Name, "Set Up");
        }

        [SerializeField] AnimatorController _animatorController;

        readonly Dictionary<string, bool> _enabledLayersByName = new();
        readonly Dictionary<string, int> _layerIdsByName = new();
        readonly Dictionary<string, EmoteLayerIdScriptableObject> _layerIdScriptableObjectsByName = new();

        AnimatorController _selectedController;
        AnimatorControllerAssets _assets;

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

        void OnWizardUpdate()
        {
            if (_selectedController == _animatorController) return;
            _selectedController = _animatorController;
            if (_selectedController == null) return;

            _assets = new AnimatorControllerAssets(_selectedController);
            
            LoadScriptableObjects();
            UpdateEnabledLayers();
            UpdateLayerIds();
        }

        void OnWizardCreate()
        {
            if (_selectedController == null)
            {
                Debug.LogWarning("No controller selected.");
                return;
            }

            var animationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClip.Name);
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
                    scriptableObject = CreateInstance<EmoteLayerIdScriptableObject>();
                    _assets.AddScriptableObject(layer.name, scriptableObject);
                }

                scriptableObject.Id = layerId;
                EditorUtility.SetDirty(scriptableObject);
                AssetDatabase.SaveAssetIfDirty(scriptableObject);
            }

            AssetDatabase.SaveAssets();
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

            var scriptableObjects = _assets
                .GetScriptableObjects<EmoteLayerIdScriptableObject>();

            foreach (var scriptableObject in scriptableObjects)
                _layerIdScriptableObjectsByName[scriptableObject.name] = scriptableObject;
        }
    }
}