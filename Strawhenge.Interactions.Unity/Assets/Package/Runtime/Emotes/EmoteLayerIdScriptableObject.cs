using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteLayerIdScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;

        public int Id => _id;
    }
}