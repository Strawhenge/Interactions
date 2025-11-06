using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Barks/Random Clip Bark")]
    public class RandomClipBarkScriptableObject : BarkScriptableObject
    {
        [SerializeField] AudioClip[] _clips;

        public override AudioClip GetAudioClip()
        {
            var index = Random.Range(0, _clips.Length);

            return _clips[index];
        }
    }
}