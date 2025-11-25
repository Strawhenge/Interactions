using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public  class SitTypeScriptableObject : ScriptableObject
    {
        internal const int DefaultId = 0;
        
        internal static string IdFieldName => nameof(_id);
        
        [SerializeField] int _id;
        
        public int Id => _id;
    }
}