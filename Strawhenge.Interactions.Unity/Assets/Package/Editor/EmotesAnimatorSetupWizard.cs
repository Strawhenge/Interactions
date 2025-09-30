using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    public class EmotesAnimatorSetupWizard : ScriptableWizard
    {
        const string Name = "Emotes Animator Setup";

        [MenuItem("Strawhenge/" + Name)]
        public static void ShowEditorWindow()
        {
            DisplayWizard<EmotesAnimatorSetupWizard>(Name, "Set Up");
        }

        [SerializeField] AnimatorController _animatorController;

        void OnWizardCreate()
        {
            var path = AssetDatabase.GetAssetPath(_animatorController);
            var parentFolder = path[..path.LastIndexOf('/')];
            var folder = parentFolder + "/" + _animatorController.name;

            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder(parentFolder, _animatorController.name);

            var animationClipPath = $"{folder}/Emote.anim";

            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);
            if (animationClip == null)
            {
                animationClip = new AnimationClip();
                AssetDatabase.CreateAsset(animationClip, animationClipPath);
            }

            EmotesAnimatorSetup.Setup(_animatorController, animationClip);
        }
    }
}