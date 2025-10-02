using UnityEngine;

namespace Strawhenge.Interactions.Unity.Emotes
{
    public class EmoteLayerIdScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;

        public int Id
        {
            get => _id;
            set => _id = value; // TODO Change setter to internal and allow Editor access to internals.
        }
    }
}