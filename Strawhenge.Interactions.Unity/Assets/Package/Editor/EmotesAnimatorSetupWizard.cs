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

        void OnWizardCreate()
        {
            EmotesAnimatorSetup.Setup(_animatorController);
        }
    }
}