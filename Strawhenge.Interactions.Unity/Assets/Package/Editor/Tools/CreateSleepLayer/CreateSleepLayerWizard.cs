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

        void OnWizardUpdate()
        {
            isValid = _animatorController != null;

            // TODO Check for existing layer
        }

        void OnWizardCreate()
        {
            CreateSleepLayer.Create(_animatorController);
        }
    }
}