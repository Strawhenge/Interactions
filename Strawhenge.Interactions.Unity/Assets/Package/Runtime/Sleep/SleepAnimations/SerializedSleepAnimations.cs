using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    [Serializable]
    public class SerializedSleepAnimations : ISleepAnimations
    {
        [SerializeField] AnimationClip _layDown;
        [SerializeField] AnimationClip _sleeping;
        [SerializeField] AnimationClip _getUp;

        public AnimationClip LayDown => _layDown ?? new AnimationClip();

        public AnimationClip Sleeping => _sleeping ?? new AnimationClip();

        public AnimationClip GetUp => _getUp ?? new AnimationClip();
    }
}