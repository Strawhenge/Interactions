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
            controller.Began += OnBegan;
            controller.Ended += OnEnded;
            return controller;
        }

        void OnBegan()
        {
            enabled = true;
        }

        void OnEnded()
        {
            enabled = false;
        }

        void FixedUpdate()
        {
            _positionPlacementController.Update(Time.fixedDeltaTime);
        }

        void OnDestroy()
        {
            _positionPlacementController.Began -= OnBegan;
            _positionPlacementController.Ended -= OnEnded;
        }
    }
}