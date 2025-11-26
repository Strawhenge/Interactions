using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class CreateSitTypeWizard : ScriptableWizard
    {
        const string Name = "Sit Type...";

        [MenuItem("Assets/Create/Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<CreateSitTypeWizard>(Name, "Create");
        }

        [SerializeField] AnimatorController _animatorController;
        [SerializeField] string _name;
        [SerializeField] AnimationClip _sitAnimation;
        [SerializeField] AnimationClip _sittingAnimation;
        [SerializeField] AnimationClip _standAnimation;

        void OnWizardUpdate()
        {
            isValid =
                _animatorController != null &&
                !string.IsNullOrEmpty(_name) &&
                _sitAnimation != null &&
                _sittingAnimation != null &&
                _standAnimation != null;
        }

        void OnWizardCreate()
        {
            CreateSitType.Create(
                _animatorController,
                _name,
                _sitAnimation,
                _sittingAnimation,
                _standAnimation);
        }
    }
}