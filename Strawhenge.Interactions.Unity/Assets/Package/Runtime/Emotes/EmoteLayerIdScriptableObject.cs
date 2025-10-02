using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteLayerIdScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;

        public int Id
        {
            get => _id;
            internal set => _id = value;
        }
    }
}