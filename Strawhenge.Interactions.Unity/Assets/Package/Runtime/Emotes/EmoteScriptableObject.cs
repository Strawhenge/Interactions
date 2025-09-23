using FunctionalUtilities;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Emote")]
    public class EmoteScriptableObject : ScriptableObject
    {
        [SerializeField] AnimationClip _animation;

        Maybe<AnimationClip> Animation => Maybe.NotNull(_animation);
    }
}