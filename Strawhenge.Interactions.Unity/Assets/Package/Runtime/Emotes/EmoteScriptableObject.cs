using FunctionalUtilities;
using Strawhenge.Common;
using Strawhenge.Interactions.Unity.Package.Runtime;
using Strawhenge.Inventory.Unity.Items.ItemData;
using System.Collections.Generic;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Emote")]
    public partial class EmoteScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;
        [SerializeField] bool _useRootMotion;
        [SerializeField] ItemScriptableObject _item;
        [SerializeField] BarkScriptableObject _bark;
        
        // TODO Remove below fields
        [SerializeField] AnimationClip _animation;
        [SerializeField] EmoteLayerIdScriptableObject _layer;
        [SerializeField] bool _isRepeating;
        [SerializeField] AnimatorBoolParameterScriptableObject[] _animatorBoolParameters;

        public Maybe<AnimationClip> Animation => Maybe.NotNull(_animation);

        public int LayerId => _layer != null ? _layer.Id : 0;

        public bool IsRepeating => _isRepeating;

        public Maybe<BarkScriptableObject> Bark => Maybe.NotNull(_bark);

        public Maybe<ItemScriptableObject> Item => Maybe.NotNull(_item);

        public IEnumerable<AnimatorBoolParameterScriptableObject> AnimatorBoolParameters =>
            _animatorBoolParameters.ExcludeNull();
    }
}