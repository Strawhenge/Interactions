using Strawhenge.Common.Unity;
using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class BarkScript : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;
        [SerializeField] LoggerScript _logger;

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

            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new BarkController(_audioSource, logger);
        }
    }
}