using FunctionalUtilities;
using Strawhenge.Inventory.Unity.Items.ItemData;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Emote")]
    public class EmoteScriptableObject : ScriptableObject
    {
        [SerializeField] AnimationClip _animation;
        [SerializeField] ItemScriptableObject _item;

        public Maybe<AnimationClip> Animation => Maybe.NotNull(_animation);

        public Maybe<ItemScriptableObject> Item => Maybe.NotNull(_item);
    }
}