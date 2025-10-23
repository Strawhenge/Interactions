using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Common.Unity.Serialization;
using System;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sleep
{
    public class SleepScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        [SerializeField] SerializedSource<
            ISleepAnimations,
            SerializedSleepAnimations,
            SleepAnimationsScriptableObject> _defaultAnimations;

        SleepController _sleepController;

        public SleepController SleepController => _sleepController ??= CreateSleepController();

        void Awake()
        {
            _sleepController ??= CreateSleepController();
        }

        SleepController CreateSleepController()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            return new SleepController(_animator, _defaultAnimations.GetValue());
        }
    }
}