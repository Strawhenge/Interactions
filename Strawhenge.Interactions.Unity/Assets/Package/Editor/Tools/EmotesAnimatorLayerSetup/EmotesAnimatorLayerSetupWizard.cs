using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class EmotesAnimatorLayerSetupWizard : ScriptableWizard
    {
        const string Name = "Emotes Animator Layer";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<EmotesAnimatorLayerSetupWizard>(Name, "Create");
        }

        [SerializeField] AnimatorController _animatorController;
        [SerializeField] string _layerName;
        [SerializeField] AvatarMask _avatarMask;

        void OnWizardCreate()
        {
            if (_animatorController == null)
            {
                Debug.LogWarning("Animator controller is not set.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_layerName))
            {
                Debug.LogWarning("Layer name is not set.");
                return;
            }
            
            EmotesAnimatorLayerSetup.Setup(_animatorController, _layerName, _avatarMask);
        }
    }
}