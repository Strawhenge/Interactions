using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Helpers;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] SleepTypeScriptableObject _defaultSleepType;
        [SerializeField] LoggerScript _logger;

        SleepController _sleepController;

        public SleepController SleepController => _sleepController ??= CreateSleepController();

        void Awake()
        {
            _sleepController ??= CreateSleepController();
        }

        SleepController CreateSleepController()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            if (_defaultSleepType == null)
                logger.LogError($"'{nameof(_defaultSleepType)}' is not set.");

            return new SleepController(_animator, _defaultSleepType, logger);
        }
    }
}