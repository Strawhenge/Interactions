using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class CreateSleepTypeWizard : ScriptableWizard
    {
        const string Name = "Sleep Type...";

        [MenuItem("Assets/Create/Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<CreateSleepTypeWizard>(Name, "Create");
        }

        [SerializeField] AnimatorController _animatorController;
        [SerializeField] string _name;
        [SerializeField] AnimationClip _layDownAnimation;
        [SerializeField] AnimationClip _sleepingAnimation;
        [SerializeField] AnimationClip _wakeUpAnimation;

        void OnEnable()
        {
            _animatorController = LastUsed.AnimatorController;
        }

        void OnWizardUpdate()
        {
            isValid =
                _animatorController != null &&
                !string.IsNullOrEmpty(_name) &&
                _layDownAnimation != null &&
                _sleepingAnimation != null &&
                _wakeUpAnimation != null;

            if (_animatorController != null)
                LastUsed.AnimatorController = _animatorController;
        }

        void OnWizardCreate()
        {
            CreateSleepType.Create(
                _animatorController,
                _name,
                _layDownAnimation,
                _sleepingAnimation,
                _wakeUpAnimation);
        }
    }
}