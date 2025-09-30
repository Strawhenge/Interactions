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
        [SerializeField] bool _isRepeating;
        [SerializeField] ItemScriptableObject _item;
        [SerializeField] int _layerId;
        [SerializeField] AnimatorBoolParameterScriptableObject[] _animatorBoolParameters;

        public Maybe<AnimationClip> Animation => Maybe.NotNull(_animation);

        public bool IsRepeating => _isRepeating;

        public Maybe<ItemScriptableObject> Item => Maybe.NotNull(_item);

        public int LayerId => _layerId;

        public IEnumerable<AnimatorBoolParameterScriptableObject> AnimatorBoolParameters =>
            _animatorBoolParameters.ExcludeNull();
    }
}