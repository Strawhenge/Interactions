using Strawhenge.Common;
using Strawhenge.Interactions.Unity.Sleep;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class SleepAnimatorSetupWizard : ScriptableWizard
    {
        const string Name = "Sleep Animator Setup";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<SleepAnimatorSetupWizard>(Name, "Set Up");
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

            _assets = new AnimatorControllerAssets(_selectedController);
            _layers = _selectedController.layers.ToArray(layer => layer.name);
        }

        void OnWizardCreate()
        {
            if (_selectedController == null)
            {
                Debug.LogWarning("No controller selected.");
                return;
            }

            var layDownAnimationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClips.LayDown);
            var sleepingAnimationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClips.Sleeping);
            var getUpAnimationClip = _assets.GetOrCreateAnimationClip(PlaceholderAnimationClips.GetUp);

            SleepAnimatorSetup.Setup(
                _animatorController,
                _selectedLayerIndex,
                layDownAnimationClip,
                sleepingAnimationClip,
                getUpAnimationClip);
        }
    }
}