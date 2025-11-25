using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepTypeScriptableObject : ScriptableObject
    {
        internal static string IdFieldName => nameof(_id);

        [SerializeField] int _id;

        public int Id => _id;
    }
}