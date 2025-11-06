using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class BarkScript : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

        BarkController _barkController;

        public BarkController BarkController => _barkController ??= CreateBarkController();

        void Awake()
        {
            _barkController ??= CreateBarkController();
        }

        BarkController CreateBarkController()
        {
            if (_audioSource == null)
            {
                Debug.LogWarning("Audio source not set.");
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            return new BarkController(_audioSource, null);
        }
    }
}