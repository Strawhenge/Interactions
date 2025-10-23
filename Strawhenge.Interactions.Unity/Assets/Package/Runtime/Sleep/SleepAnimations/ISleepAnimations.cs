using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public interface ISleepAnimations
    {
        AnimationClip LayDown { get; }

        AnimationClip Sleeping { get; }

        AnimationClip GetUp { get; }
    }
}