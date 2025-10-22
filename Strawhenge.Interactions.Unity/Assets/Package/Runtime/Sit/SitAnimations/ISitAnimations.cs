using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public interface ISitAnimations
    {
        AnimationClip Sit { get; }

        AnimationClip Sitting { get; }

        AnimationClip Stand { get; }
    }
}