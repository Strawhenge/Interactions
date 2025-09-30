using FunctionalUtilities;
using Strawhenge.Common;
using Strawhenge.Interactions.Unity.Package.Runtime;
using Strawhenge.Inventory.Unity.Items.ItemData;
using System.Collections.Generic;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Emote")]
    public class EmoteScriptableObject : ScriptableObject
    {
        [SerializeField] AnimationClip _animation;
        [SerializeField] ItemScriptableObject _item;
        [SerializeField] AnimatorBoolParameterScriptableObject[] _animatorBoolParameters;

        public Maybe<AnimationClip> Animation => Maybe.NotNull(_animation);

        public Maybe<ItemScriptableObject> Item => Maybe.NotNull(_item);

        public IEnumerable<AnimatorBoolParameterScriptableObject> AnimatorBoolParameters =>
            _animatorBoolParameters.ExcludeNull();
    }
}