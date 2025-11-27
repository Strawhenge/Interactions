using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class CreateEmotesLayerWizard : ScriptableWizard
    {
        const string Name = "Emotes Animator Layer";

        [MenuItem("Strawhenge/Interactions/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<CreateEmotesLayerWizard>(Name, "Create");
        }

        [SerializeField] AnimatorController _animatorController;
        [SerializeField] string _layerName;
        [SerializeField] AvatarMask _avatarMask;

        void OnEnable()
        {
            _animatorController = LastUsed.AnimatorController;
        }

        void OnWizardUpdate()
        {
            isValid =
                _animatorController != null &&
                !string.IsNullOrWhiteSpace(_layerName);

            if (_animatorController != null)
                LastUsed.AnimatorController = _animatorController;
        }

        void OnWizardCreate()
        {
            CreateEmotesLayer.Create(_animatorController, _layerName, _avatarMask);
        }
    }
}