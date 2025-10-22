using Strawhenge.Common;
using Strawhenge.Interactions.Unity.Sit;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class SitAnimatorSetupWizard : ScriptableWizard
    {
        const string Name = "Sit Animator Setup";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<SitAnimatorSetupWizard>(Name, "Set Up");
        }

        [SerializeField] AnimatorController _animatorController;

        AnimatorController _selectedController;
        string[] _layers;
        int _selectedLayerIndex;
        AnimatorControllerAssets _assets;

        protected override bool DrawWizardGUI()
        {
            if (_selectedController == null)
                return base.DrawWizardGUI();

            var result = base.DrawWizardGUI();

            _selectedLayerIndex = EditorGUILayout.Popup("Layer", _selectedLayerIndex, _layers);

            return result;
        }

        void OnWizardUpdate()
        {
            if (_selectedController == _animatorController) return;
            _selectedController = _animatorController;

            if (_selectedController == null) return;

            UpdateAssetsFolder();
            UpdateLayers();
        }

        void OnWizardCreate()
        {
            if (_selectedController == null)
            {
                Debug.LogWarning("No controller selected.");
                return;
            }

            var sitAnimationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClips.Sit);
            var sittingAnimationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClips.Sitting);
            var standAnimationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClips.Stand);

            SitAnimatorSetup.Setup(
                _animatorController,
                _selectedLayerIndex,
                sitAnimationClip,
                sittingAnimationClip,
                standAnimationClip);
        }

        void UpdateLayers()
        {
            _layers = _selectedController.layers.ToArray(layer => layer.name);
        }

        void UpdateAssetsFolder()
        {
            _assets = new AnimatorControllerAssets(_selectedController);
        }
    }
}