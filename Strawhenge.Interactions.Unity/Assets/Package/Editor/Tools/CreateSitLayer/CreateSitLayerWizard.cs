using Strawhenge.Interactions.Unity.Sit;
using System.Linq;
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
            if (_animatorController == null)
            {
                isValid = false;
                return;
            }

            if (_animatorController.GetLayersContaining<SitStateMachine>().Any())
            {
                isValid = false;
                errorString = "Sit layer already exists.";
                return;
            }

            isValid = true;
            errorString = string.Empty;
        }

        void OnWizardCreate()
        {
            CreateSitLayer.Create(_animatorController);
        }
    }
}