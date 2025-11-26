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

        void OnWizardUpdate()
        {
            isValid =
                _animatorController != null &&
                !string.IsNullOrWhiteSpace(_layerName);
        }

        void OnWizardCreate()
        {
            CreateEmotesLayer.Create(_animatorController, _layerName, _avatarMask);
        }
    }
}