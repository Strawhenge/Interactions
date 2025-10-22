using Strawhenge.Common.Unity.Helpers;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        SitController _sitController;

        public SitController SitController => _sitController ??= CreateSitController();

        void Awake()
        {
            _sitController ??= CreateSitController();
        }

        SitController CreateSitController()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            return new SitController(_animator);
        }
    }
}