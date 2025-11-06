using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public abstract class BarkScriptableObject : ScriptableObject
    {
        public abstract AudioClip GetAudioClip();
    }
}