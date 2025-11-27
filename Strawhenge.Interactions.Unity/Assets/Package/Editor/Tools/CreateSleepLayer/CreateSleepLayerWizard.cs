using Strawhenge.Interactions.Unity.Sleep;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class CreateSleepLayerWizard : ScriptableWizard
    {
        const string Name = "Sleep Animator Layer";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<CreateSleepLayerWizard>(Name, "Create");
        }

        [SerializeField] AnimatorController _animatorController;

        void OnEnable()
        {
            _animatorController = LastUsed.AnimatorController;
        }

        void OnWizardUpdate()
        {
            if (_animatorController == null)
            {
                isValid = false;
                return;
            }

            LastUsed.AnimatorController = _animatorController;

            if (_animatorController.GetLayersContaining<SleepStateMachine>().Any())
            {
                isValid = false;
                errorString = "Sleep layer already exists.";
                return;
            }

            isValid = true;
            errorString = string.Empty;
        }

        void OnWizardCreate()
        {
            CreateSleepLayer.Create(_animatorController);
        }
    }
}