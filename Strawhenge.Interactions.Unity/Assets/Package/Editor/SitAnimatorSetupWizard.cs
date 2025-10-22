using Strawhenge.Common;
using Strawhenge.Interactions.Unity.Sit;
using System.Linq;
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
        string _assetsParentFolder;
        string _assetsFolder;

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

            EnsureAssetsFolderExists();
            var sitAnimationClip = GetOrCreatePlaceholderAnimationClip(PlaceholderAnimationClips.Sit);
            var sittingAnimationClip = GetOrCreatePlaceholderAnimationClip(PlaceholderAnimationClips.Sitting);
            var standAnimationClip = GetOrCreatePlaceholderAnimationClip(PlaceholderAnimationClips.Stand);

            SitAnimatorSetup.Setup(
                _animatorController,
                _selectedLayerIndex,
                sitAnimationClip,
                sittingAnimationClip,
                standAnimationClip);
        }

        AnimationClip GetOrCreatePlaceholderAnimationClip(string clipName)
        {
            var animationClipPath = $"{_assetsFolder}/{clipName}.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            return animationClip;
        }

        void EnsureAssetsFolderExists()
        {
            if (!AssetDatabase.IsValidFolder(_assetsFolder))
                AssetDatabase.CreateFolder(_assetsParentFolder, _animatorController.name);
        }

        void UpdateLayers()
        {
            _layers = _selectedController.layers.ToArray(layer => layer.name);
        }

        void UpdateAssetsFolder()
        {
            var path = AssetDatabase.GetAssetPath(_animatorController);
            _assetsParentFolder = path[..path.LastIndexOf('/')];
            _assetsFolder = $"{_assetsParentFolder}/{_animatorController.name}";
        }
    }
}