using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitTypeScriptableObject : ScriptableObject
    {
        internal static string IdFieldName => nameof(_id);

        [SerializeField] int _id;

        public int Id => _id;
    }
}