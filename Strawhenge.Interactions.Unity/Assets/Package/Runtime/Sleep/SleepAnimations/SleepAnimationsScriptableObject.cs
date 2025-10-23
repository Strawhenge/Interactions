using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    [CreateAssetMenu(menuName = "Strawhenge/Interactions/Sleep Animations")]
    public class SleepAnimationsScriptableObject : ScriptableObject, ISleepAnimations
    {
        [SerializeField] AnimationClip _layDown;
        [SerializeField] AnimationClip _sleeping;
        [SerializeField] AnimationClip _getUp;

        public AnimationClip LayDown => _layDown ?? new AnimationClip();

        public AnimationClip Sleeping => _sleeping ?? new AnimationClip();

        public AnimationClip GetUp => _getUp ?? new AnimationClip();
    }
}