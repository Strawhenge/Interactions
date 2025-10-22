using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    [Serializable]
    public class SerializedSitAnimations : ISitAnimations
    {
        [SerializeField] AnimationClip _sit;
        [SerializeField] AnimationClip _sitting;
        [SerializeField] AnimationClip _stand;

        public AnimationClip Sit => _sit ?? new AnimationClip();

        public AnimationClip Sitting => _sitting ?? new AnimationClip();

        public AnimationClip Stand => _stand ?? new AnimationClip();
    }
}