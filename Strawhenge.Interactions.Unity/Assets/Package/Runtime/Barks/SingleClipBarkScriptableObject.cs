using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Barks/Single Clip Bark")]
    public class SingleClipBarkScriptableObject : BarkScriptableObject
    {
        [SerializeField] AudioClip _clip;

        public override AudioClip GetAudioClip()
        {
            return _clip;
        }
    }
}