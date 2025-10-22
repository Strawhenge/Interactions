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

        public AnimationClip Sit => _sit ?? MissingAnimation(nameof(_sit));

        public AnimationClip Sitting => _sitting ?? MissingAnimation(nameof(_sitting));

        public AnimationClip Stand => _stand ?? MissingAnimation(nameof(_stand));

        AnimationClip MissingAnimation(string fieldName)
        {
            Debug.LogError($"Missing animation clip '{fieldName}'.");
            return new AnimationClip();
        }
    }
}