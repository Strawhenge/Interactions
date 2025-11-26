using Strawhenge.Common.Unity;
using Strawhenge.Common.Unity.Helpers;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] SitTypeScriptableObject _defaultSitType;
        [SerializeField, Tooltip("Optional.")] LoggerScript _logger;

        SitController _sitController;

        public SitController SitController => _sitController ??= CreateSitController();

        void Awake()
        {
            _sitController ??= CreateSitController();
        }

        SitController CreateSitController()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            if (_defaultSitType == null)
                Debug.LogError($"'{nameof(_defaultSitType)}' not set.", this);

            return new SitController(_animator, _defaultSitType, logger);
        }
    }
}