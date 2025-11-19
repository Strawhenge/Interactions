using Strawhenge.Interactions.Unity.Emotes;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class AddEmoteWizard : ScriptableWizard
    {
        const string Name = "Add Emote";

        [MenuItem("Assets/Create/Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<AddEmoteWizard>(Name, "Create");
        }

        AnimatorController _animatorController;
        string[] _layerNames = Array.Empty<string>();
        int _selectedLayerIndex;
        string _emoteName;
        AnimationClip _animation;
        bool _isRepeating;
        bool _useRootMotion;

        protected override bool DrawWizardGUI()
        {
            var result = base.DrawWizardGUI();

            var animatorController = EditorGUILayout.ObjectField(
                label: "Animator Controller",
                obj: _animatorController,
                objType: typeof(AnimatorController),
                allowSceneObjects: false) as AnimatorController;

            if (_animatorController != animatorController)
                _layerNames = GetEmoteLayers(animatorController);
            _animatorController = animatorController;

            _selectedLayerIndex = EditorGUILayout.Popup(
                label: "Layer",
                selectedIndex: _selectedLayerIndex,
                displayedOptions: _layerNames);

            _emoteName = EditorGUILayout.TextField(
                label: "Emote Name",
                text: _emoteName);

            _animation = EditorGUILayout.ObjectField(
                label: "Animation",
                obj: _animation,
                objType: typeof(AnimationClip),
                allowSceneObjects: false) as AnimationClip;

            _isRepeating = EditorGUILayout.Toggle(
                label: "Is Repeating",
                _isRepeating);

            _useRootMotion = EditorGUILayout.Toggle(
                label: "Use Root Motion",
                _useRootMotion);

            return result;
        }

        void OnWizardCreate()
        {
            // TODO add validation

            AddEmote.Add(new AddEmoteArgs(
                _animatorController,
                _layerNames[_selectedLayerIndex],
                _emoteName,
                _animation,
                _isRepeating,
                _useRootMotion));
        }

        static string[] GetEmoteLayers(AnimatorController animatorController)
        {
            return animatorController != null
                ? animatorController
                    .GetEmoteLayers()
                    .Select(x => x.name)
                    .ToArray()
                : Array.Empty<string>();
        }
    }
}