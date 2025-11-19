using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    class AddEmoteArgs
    {
        public AddEmoteArgs(
            AnimatorController animatorController,
            string layerName,
            string emoteName,
            AnimationClip animation,
            bool isRepeating,
            bool useRootMotion)
        {
            AnimatorController = animatorController;
            LayerName = layerName;
            EmoteName = emoteName;
            Animation = animation;
            IsRepeating = isRepeating;
            UseRootMotion = useRootMotion;
        }

        public AnimatorController AnimatorController { get; }

        public string LayerName { get; }

        public string EmoteName { get; }

        public AnimationClip Animation { get; }

        public bool IsRepeating { get; }

        public bool UseRootMotion { get; }
    }
}