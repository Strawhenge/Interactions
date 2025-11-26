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
            _animationHandler.Sitting += OnSitting;
            _animationHandler.Standing += OnStanding;
            
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

            _animationHandler.Sit(sitType ?? _defaultSitType);
        }

        public void Stand()
        {
            if (!IsSitting) return;
           
            _animationHandler.Stand();
        }

        void OnSitting()
        {
            Sitting?.Invoke();
        }

        void OnStanding()
        {
            IsSitting = false;
            Standing?.Invoke();
        }
    }
}