using Strawhenge.Common.Unity.Helpers;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] SleepTypeScriptableObject _defaultSleepType;

        SleepController _sleepController;

        public SleepController SleepController => _sleepController ??= CreateSleepController();

        void Awake()
        {
            _sleepController ??= CreateSleepController();
        }

        SleepController CreateSleepController()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            if (_defaultSleepType == null)
                Debug.LogError($"'{nameof(_defaultSleepType)}' is not set.", this);

            return new SleepController(_animator, _defaultSleepType);
        }
    }
}