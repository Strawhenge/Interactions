using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity
{
    public class BarkController
    {
        readonly AudioSource _audioSource;
        readonly ILogger _logger;

        public BarkController(AudioSource audioSource, ILogger logger)
        {
            _audioSource = audioSource;
            _logger = logger;
        }

        public bool IsPlaying => _audioSource.isPlaying;

        public void Play(BarkScriptableObject bark)
        {
            _logger.LogInformation($"Playing bark '{bark.name}'.");
            
            _audioSource.clip = bark.GetAudioClip();
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}