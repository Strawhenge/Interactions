using Strawhenge.Inventory.Unity.Items.ItemData;
using UnityEditor.Animations;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Editor
{
    class CreateEmoteArgs
    {
        public CreateEmoteArgs(
            AnimatorController animatorController,
            string layerName,
            string emoteName,
            AnimationClip animation,
            bool isRepeating,
            bool useRootMotion,
            ItemScriptableObject item,
            BarkScriptableObject bark)
        {
            AnimatorController = animatorController;
            LayerName = layerName;
            EmoteName = emoteName;
            Animation = animation;
            IsRepeating = isRepeating;
            UseRootMotion = useRootMotion;
            Item = item;
            Bark = bark;
        }

        public AnimatorController AnimatorController { get; }

        public string LayerName { get; }

        public string EmoteName { get; }

        public AnimationClip Animation { get; }

        public bool IsRepeating { get; }

        public bool UseRootMotion { get; }

        public ItemScriptableObject Item { get; }

        public BarkScriptableObject Bark { get; }
    }
}