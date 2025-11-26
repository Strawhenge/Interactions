using FunctionalUtilities;
using Strawhenge.Inventory.Unity.Items.ItemData;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public partial class EmoteScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;
        [SerializeField] bool _useRootMotion;
        [SerializeField, Tooltip("Optional.")] ItemScriptableObject _item;
        [SerializeField, Tooltip("Optional.")] BarkScriptableObject _bark;

        public int Id => _id;

        public bool UseRootMotion => _useRootMotion;

        public Maybe<BarkScriptableObject> Bark => Maybe.NotNull(_bark);

        public Maybe<ItemScriptableObject> Item => Maybe.NotNull(_item);
    }
}