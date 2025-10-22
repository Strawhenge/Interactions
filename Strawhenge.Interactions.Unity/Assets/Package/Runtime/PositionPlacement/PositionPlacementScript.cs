using UnityEngine;

namespace Strawhenge.Interactions.Unity.PositionPlacement
{
    public class PositionPlacementScript : MonoBehaviour
    {
        [SerializeField, Tooltip("Optional. Will use 'this' transform if not provided.")]
        Transform _root;

        PositionPlacementController _positionPlacementController;

        public PositionPlacementController PositionPlacementController =>
            _positionPlacementController ??= CreateController();

        void Awake()
        {
            enabled = false;
            _positionPlacementController ??= CreateController();
        }

        PositionPlacementController CreateController()
        {
            var controller = new PositionPlacementController(_root);
            controller.IsInProgressChanged += OnIsInProgressChanged;
            return controller;
        }

        void OnIsInProgressChanged()
        {
            enabled = _positionPlacementController.IsInProgress;
        }

        void FixedUpdate()
        {
            _positionPlacementController.Update(Time.fixedDeltaTime);
        }

        void OnDestroy()
        {
            _positionPlacementController.IsInProgressChanged -= OnIsInProgressChanged;
        }
    }
}