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
        [SerializeField] AnimationClip _defaultSitAnimation;
        [SerializeField] AnimationClip _defaultSittingAnimation;
        [SerializeField] AnimationClip _defaultStandAnimation;

        void OnWizardUpdate()
        {
            isValid =
                _animatorController != null &&
                _defaultSitAnimation != null &&
                _defaultSittingAnimation != null &&
                _defaultStandAnimation != null;
        }

        void OnWizardCreate()
        {
            CreateSitLayer.Create(
                _animatorController,
                _defaultSitAnimation,
                _defaultSittingAnimation,
                _defaultStandAnimation);
        }
    }
}