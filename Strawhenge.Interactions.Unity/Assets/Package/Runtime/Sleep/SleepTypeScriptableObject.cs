using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepTypeScriptableObject : ScriptableObject
    {
        [SerializeField] int _id;

        public int Id => _id;
    }
}