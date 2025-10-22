using Strawhenge.Common.Unity.Helpers;
using Strawhenge.Common.Unity.Serialization;
using UnityEngine;

namespace Strawhenge.Interactions.Unity.Sit
{
    public class SitScript : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        [SerializeField] SerializedSource<
            ISitAnimations,
            SerializedSitAnimations,
            SitAnimationsScriptableObject> _defaultAnimations;

        SitController _sitController;

        public SitController SitController => _sitController ??= CreateSitController();

        void Awake()
        {
            _sitController ??= CreateSitController();
        }

        SitController CreateSitController()
        {
            ComponentRefHelper.EnsureRootHierarchyComponent(ref _animator, nameof(_animator), this);

            return new SitController(_animator, _defaultAnimations.GetValue());
        }
    }
}