using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    class AnimatorControllerAssets
    {
        readonly string _name;
        readonly string _parentFolder;
        readonly string _folder;

        public AnimatorControllerAssets(AnimatorController animatorController)
        {
            _name = animatorController.name;

            var path = AssetDatabase.GetAssetPath(animatorController);
            _parentFolder = path[..path.LastIndexOf('/')];

            _folder = $"{_parentFolder}/{_name}";
        }

        public IReadOnlyList<T> GetScriptableObjects<T>() where T : ScriptableObject
        {
            if (!AssetDatabase.IsValidFolder(_folder))
                return Array.Empty<T>();

            // ReSharper disable once UseNameOfInsteadOfTypeOf
            // Justification: `nameof` does not work.
            var assetGuids = AssetDatabase.FindAssets(
                $"t:{typeof(T).Name}",
                new[] { _folder });

            var assets = new List<T>();

            foreach (var assetGuid in assetGuids)
            {
                var scriptableObject = AssetDatabase
                    .LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assetGuid));

                if (scriptableObject != null)
                    assets.Add(scriptableObject);
            }

            return assets;
        }

        public void AddScriptableObject(string name, ScriptableObject scriptableObject)
        {
            var scriptableObjectPath = $"{_folder}/{name}.asset";
            AssetDatabase.CreateAsset(scriptableObject, scriptableObjectPath);
        }

        public AnimationClip GetOrCreateAnimationClip(string clipName)
        {
            var animationClipPath = $"{_folder}/{clipName}.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                EnsureAssetsFolderExists();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            return animationClip;
        }

        void EnsureAssetsFolderExists()
        {
            if (!AssetDatabase.IsValidFolder(_folder))
                AssetDatabase.CreateFolder(_parentFolder, _name);
        }
    }
}