using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class CreateSitLayerWizard : ScriptableWizard
    {
        const string Name = "Sit Animator Layer";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<CreateSitLayerWizard>(Name, "Create");
        }

        [SerializeField] AnimatorController _animatorController;

        void OnWizardUpdate()
        {
            isValid = _animatorController != null;
            
            // TODO Check for existing layer
        }

        void OnWizardCreate()
        {
            CreateSitLayer.Create(_animatorController);
        }
    }
}