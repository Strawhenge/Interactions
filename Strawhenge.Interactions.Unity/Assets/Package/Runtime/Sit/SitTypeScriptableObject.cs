using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public partial class SitTypeScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;
        
        public int Id => _id;
    }
}