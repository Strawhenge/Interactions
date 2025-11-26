using System;
using UnityEngine;
using ILogger = Strawhenge.Common.Logging.ILogger;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitController
    {
        readonly SitAnimationHandler _animationHandler;
        readonly SitTypeScriptableObject _defaultSitType;
        readonly ILogger _logger;

        public SitController(
            Animator animator,
            SitTypeScriptableObject defaultSitType,
            ILogger logger)
        {
            _animationHandler = new SitAnimationHandler(animator, logger);
            _defaultSitType = defaultSitType;
            _logger = logger;
        }

        public bool IsSitting { get; private set; }

        public event Action Sitting;

        public event Action Standing;

        public void Sit(SitTypeScriptableObject sitType = null)
        {
            if (IsSitting)
            {
                _logger.LogWarning("Already sitting.");
                return;
            }

            IsSitting = true;

            _animationHandler.Sitting += OnSitting;
            _animationHandler.Standing += OnStanding;

            _animationHandler.Sit(sitType ?? _defaultSitType);
        }

        public void Stand()
        {
            if (!IsSitting) return;

            _animationHandler.Standing += OnStanding;
            _animationHandler.Stand();
        }

        void OnSitting()
        {
            _animationHandler.Sitting -= OnSitting;
            Sitting?.Invoke();
        }

        void OnStanding()
        {
            _animationHandler.Sitting -= OnSitting;
            _animationHandler.Standing -= OnStanding;
            
            IsSitting = false;
            Standing?.Invoke();
        }
    }
}